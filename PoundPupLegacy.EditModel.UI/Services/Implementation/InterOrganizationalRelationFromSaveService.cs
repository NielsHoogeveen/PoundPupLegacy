using PoundPupLegacy.CreateModel;
using PoundPupLegacy.CreateModel.Updaters;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal class InterOrganizationalRelationFromSaveService(
    IDatabaseUpdaterFactory<NodeUnpublishRequest> nodeUnpublishFactory,
    IDatabaseUpdaterFactory<ImmediatelyIdentifiableInterOrganizationalRelation> interOrganizationalRelationUpdaterFactory,
    IEntityCreatorFactory<EventuallyIdentifiableInterOrganizationalRelation> interOrganizationalRelationCreatorFactory
) : ISaveService<IEnumerable<ResolvedInterOrganizationalRelationFrom>>
{
    public async Task SaveAsync(IEnumerable<ResolvedInterOrganizationalRelationFrom> item, IDbConnection connection)
    {
        await using var unpublisher = await nodeUnpublishFactory.CreateAsync(connection);
        await using var updater = await interOrganizationalRelationUpdaterFactory.CreateAsync(connection);

        foreach (var relation in item.OfType<ExistingInterOrganizationalRelationFrom>().Where(x => x.HasBeenDeleted)) {
            await unpublisher.UpdateAsync(new NodeUnpublishRequest {
                NodeId = relation.NodeId
            });
        }
        foreach (var relation in item.OfType<ExistingInterOrganizationalRelationFrom>().Where(x => !x.HasBeenDeleted)) {
            await updater.UpdateAsync(new ExistingInterOrganizationalRelation {
                Id = relation.NodeId,
                Title = relation.Title,
                Description = relation.Description,
                OrganizationIdFrom = relation.OrganizationFrom.Id,
                OrganizationIdTo = relation.OrganizationTo.Id,
                InterOrganizationalRelationTypeId = relation.InterOrganizationalRelationType.Id,
                DateRange = relation.DateRange ?? new DateTimeRange(null, null) ,
                GeographicalEntityId = relation.GeographicalEntity?.Id,
                DocumentIdProof = relation.ProofDocument?.Id,
                MoneyInvolved = relation.MoneyInvolved,
                NumberOfChildrenInvolved = relation.NumberOfChildrenInvolved,
                NewNodeTerms = new List<NodeTerm>(),
                NodeTermsToRemove = new List<NodeTerm>(),
                AuthoringStatusId = 1,
                ChangedDateTime = DateTime.Now,
                NewTenantNodes = new List<NewTenantNodeForExistingNode>(),
                TenantNodesToRemove = new List<ExistingTenantNode>(),
                TenantNodesToUpdate = new List<ExistingTenantNode>()
            });
        }
        IEnumerable<CreateModel.NewInterOrganizationalRelationForExistingParticipants> GetRelationsToInsert()
        {

            foreach (var relation in item.OfType<NewInterOrganizationalExistingRelationFrom>().Where(x => !x.HasBeenDeleted)) {
                var now = DateTime.Now;
                yield return new CreateModel.NewInterOrganizationalRelationForExistingParticipants {
                    Id = null,
                    PublisherId = relation.PublisherId,
                    CreatedDateTime = now,
                    ChangedDateTime = now,
                    Title = relation.Title,
                    OwnerId = relation.OwnerId,
                    AuthoringStatusId = 1,
                    TenantNodes = relation.TenantNodes.Select(tenantNode => new CreateModel.NewTenantNodeForNewNode {
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
                    NodeTermIds = new List<int>(),
                };
            }
        }
        await using var interOrganizationalRelationCreator = await interOrganizationalRelationCreatorFactory.CreateAsync(connection);
        await interOrganizationalRelationCreator.CreateAsync(GetRelationsToInsert().ToAsyncEnumerable());
    }
}