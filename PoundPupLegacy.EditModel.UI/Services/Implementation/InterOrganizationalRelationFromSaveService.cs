

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal class InterOrganizationalRelationFromSaveService : ISaveService<IEnumerable<ResolvedInterOrganizationalRelationFrom>>
{
    private readonly IDatabaseUpdaterFactory<NodeUnpublishRequest> _nodeUnpublishFactory;
    private readonly IDatabaseUpdaterFactory<InterOrganizationalRelationUpdaterRequest> _interOrganizationalRelationUpdaterFactory;
    private readonly IEntityCreator<CreateModel.InterOrganizationalRelation> _interOrganizationalRelationCreator;
    public InterOrganizationalRelationFromSaveService(
        IDatabaseUpdaterFactory<NodeUnpublishRequest> nodeUnpublishFactory,
        IDatabaseUpdaterFactory<InterOrganizationalRelationUpdaterRequest> interOrganizationalRelationUpdaterFactory,
        IEntityCreator<CreateModel.InterOrganizationalRelation> interOrganizationalRelationCreator
    )
    {
        _nodeUnpublishFactory = nodeUnpublishFactory;
        _interOrganizationalRelationUpdaterFactory = interOrganizationalRelationUpdaterFactory;
        _interOrganizationalRelationCreator = interOrganizationalRelationCreator;
    }
    public async Task SaveAsync(IEnumerable<ResolvedInterOrganizationalRelationFrom> item, IDbConnection connection)
    {
        await using var unpublisher = await _nodeUnpublishFactory.CreateAsync(connection);
        await using var updater = await _interOrganizationalRelationUpdaterFactory.CreateAsync(connection);

        foreach (var relation in item.OfType<ExistingInterOrganizationalRelationFrom>().Where(x => x.HasBeenDeleted)) {
            await unpublisher.UpdateAsync(new NodeUnpublishRequest {
                NodeId = relation.NodeId
            });
        }
        foreach (var relation in item.OfType<ExistingInterOrganizationalRelationFrom>().Where(x => !x.HasBeenDeleted)) {
            await updater.UpdateAsync(new InterOrganizationalRelationUpdaterRequest {
                NodeId = relation.NodeId,
                Title = relation.Title,
                Description = relation.Description,
                OrganizationIdFrom = relation.OrganizationFrom.Id,
                OrganizationIdTo = relation.OrganizationTo.Id,
                InterOrganizationalRelationTypeId = relation.InterOrganizationalRelationType.Id,
                DateRange = relation.DateRange,
                GeographicalEntityId = relation.GeographicalEntity?.Id,
                DocumentIdProof = relation.ProofDocument?.Id,
                MoneyInvolved = relation.MoneyInvolved,
                NumberOfChildrenInvolved = relation.NumberOfChildrenInvolved,
            });
        }
        IEnumerable<CreateModel.InterOrganizationalRelation> GetRelationsToInsert()
        {

            foreach (var relation in item.OfType<NewInterOrganizationalExistingRelationFrom>().Where(x => !x.HasBeenDeleted)) {
                var now = DateTime.Now;
                yield return new CreateModel.InterOrganizationalRelation {
                    Id = null,
                    PublisherId = relation.PublisherId,
                    CreatedDateTime = now,
                    ChangedDateTime = now,
                    Title = relation.Title,
                    OwnerId = relation.OwnerId,
                    AuthoringStatusId = 1,
                    TenantNodes = relation.TenantNodes.Select(tenantNode => new CreateModel.TenantNode {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = tenantNode.PublicationStatusId,
                        UrlPath = tenantNode.UrlPath,
                        NodeId = null,
                        SubgroupId = tenantNode.SubgroupId,
                        UrlId = null
                    }).ToList(),
                    NodeTypeId = 47,
                    OrganizationIdFrom = relation.OrganizationFrom.Id,
                    OrganizationIdTo = relation.OrganizationTo.Id,
                    GeographicalEntityId = relation.GeographicalEntity?.Id,
                    InterOrganizationalRelationTypeId = relation.InterOrganizationalRelationType.Id,
                    DateRange = relation.DateRange is null ? new DateTimeRange(null, null) : relation.DateRange,
                    DocumentIdProof = relation.ProofDocument?.Id,
                    Description = relation.Description,
                    MoneyInvolved = relation.MoneyInvolved,
                    NumberOfChildrenInvolved = relation.NumberOfChildrenInvolved,
                };
            }
        }
        await _interOrganizationalRelationCreator.CreateAsync(GetRelationsToInsert().ToAsyncEnumerable(), connection);
    }
}