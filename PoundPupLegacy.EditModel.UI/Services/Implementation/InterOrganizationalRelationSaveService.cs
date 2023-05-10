using PoundPupLegacy.CreateModel.Creators;
using PoundPupLegacy.EditModel.Updaters;
using System.Data;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal class InterOrganizationalRelationSaveService : ISaveService<IEnumerable<InterOrganizationalRelation>>
{
    private readonly IDatabaseUpdaterFactory<NodeUnpublishRequest> _nodeUnpublishFactory;
    private readonly IDatabaseUpdaterFactory<InterOrganizationalRelationUpdaterRequest> _interOrganizationalRelationUpdaterFactory;
    private readonly IEntityCreator<CreateModel.InterOrganizationalRelation> _interOrganizationalRelationCreator;
    public InterOrganizationalRelationSaveService(
        IDatabaseUpdaterFactory<NodeUnpublishRequest> nodeUnpublishFactory,
        IDatabaseUpdaterFactory<InterOrganizationalRelationUpdaterRequest> interOrganizationalRelationUpdaterFactory,
        IEntityCreator<CreateModel.InterOrganizationalRelation> interOrganizationalRelationCreator
    )
    {
        _nodeUnpublishFactory = nodeUnpublishFactory;
        _interOrganizationalRelationUpdaterFactory = interOrganizationalRelationUpdaterFactory;
        _interOrganizationalRelationCreator = interOrganizationalRelationCreator;
    }
    public async Task SaveAsync(IEnumerable<InterOrganizationalRelation> item, IDbConnection connection)
    {
        await using var unpublisher = await _nodeUnpublishFactory.CreateAsync(connection);
        await using var updater = await _interOrganizationalRelationUpdaterFactory.CreateAsync(connection);

        foreach (var relation in item.Where(x => x.HasBeenDeleted)) {
            if (!relation.NodeId.HasValue)
                throw new Exception("relation has no node id and cannot be unpublished");
            await unpublisher.UpdateAsync(new NodeUnpublishRequest {
                NodeId = relation.NodeId.Value
            });
        }
        foreach (var relation in item.Where(x => x.NodeId.HasValue && !x.HasBeenDeleted)) {
            await updater.UpdateAsync(new InterOrganizationalRelationUpdaterRequest {
                NodeId = relation.NodeId!.Value,
                Title = relation.Title,
                Description = relation.Description,
                OrganizationIdFrom = relation.OrganizationFrom.Id,
                OrganizationIdTo = relation.OrganizationTo.Id,
                InterOrganizationalRelationTypeId = relation.InterOrganizationalRelationType.Id,
                DateRange = relation.DateRange,
                GeographicalEntityId = relation.GeographicalEntity?.Id,
                DocumentIdProof = relation.DocumentProof?.Id,
                MoneyInvolved = relation.MoneyInvolved,
                NumberOfChildrenInvolved = relation.NumberOfChildrenInvolved,
            });
        }
        IEnumerable<CreateModel.InterOrganizationalRelation> GetRelationsToInsert()
        {

            foreach (var relation in item.Where(x => !x.NodeId.HasValue)) {
                var now = DateTime.Now;
                yield return new CreateModel.InterOrganizationalRelation {
                    Id = null,
                    PublisherId = relation.PublisherId,
                    CreatedDateTime = now,
                    ChangedDateTime = now,
                    Title = relation.Title,
                    OwnerId = relation.OwnerId,
                    TenantNodes = relation.TenantNodes.Select(tenantNode => new CreateModel.TenantNode
                    {
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
                    DateRange = relation.DateRange is null ? new DateTimeRange(null, null): relation.DateRange,
                    DocumentIdProof = relation.DocumentProof?.Id,
                    Description = relation.Description,
                    MoneyInvolved = relation.MoneyInvolved,
                    NumberOfChildrenInvolved = relation.NumberOfChildrenInvolved,
                };
            }
        }
        await _interOrganizationalRelationCreator.CreateAsync(GetRelationsToInsert().ToAsyncEnumerable(), connection);
    }
}