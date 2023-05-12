using PoundPupLegacy.CreateModel.Creators;
using PoundPupLegacy.EditModel.Updaters;
using System.Data;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal class InterPersonalRelationSaveService : ISaveService<IEnumerable<InterPersonalRelation>>
{
    private readonly IDatabaseUpdaterFactory<NodeUnpublishRequest> _nodeUnpublishFactory;
    private readonly IDatabaseUpdaterFactory<InterPersonalRelationUpdaterRequest> _interPersonalRelationUpdaterFactory;
    private readonly IEntityCreator<CreateModel.InterPersonalRelation> _interPersonalRelationCreator;
    public InterPersonalRelationSaveService(
        IDatabaseUpdaterFactory<NodeUnpublishRequest> nodeUnpublishFactory,
        IDatabaseUpdaterFactory<InterPersonalRelationUpdaterRequest> interPersonalRelationUpdaterFactory,
        IEntityCreator<CreateModel.InterPersonalRelation> interPersonalRelationCreator
    )
    {
        _nodeUnpublishFactory = nodeUnpublishFactory;
        _interPersonalRelationUpdaterFactory = interPersonalRelationUpdaterFactory;
        _interPersonalRelationCreator = interPersonalRelationCreator;
    }
    public async Task SaveAsync(IEnumerable<InterPersonalRelation> item, IDbConnection connection)
    {
        await using var unpublisher = await _nodeUnpublishFactory.CreateAsync(connection);
        await using var updater = await _interPersonalRelationUpdaterFactory.CreateAsync(connection);

        foreach (var relation in item.Where(x => x.HasBeenDeleted)) {
            if (!relation.NodeId.HasValue)
                throw new Exception("relation has no node id and cannot be unpublished");
            await unpublisher.UpdateAsync(new NodeUnpublishRequest {
                NodeId = relation.NodeId.Value
            });
        }
        foreach (var relation in item.Where(x => x.NodeId.HasValue && !x.HasBeenDeleted)) {
            await updater.UpdateAsync(new InterPersonalRelationUpdaterRequest {
                NodeId = relation.NodeId!.Value,
                Title = relation.Title,
                PersonIdFrom = relation.PersonFrom.Id!.Value,
                PersonIdTo = relation.PersonTo.Id!.Value,
                InterPersonalRelationTypeId = relation.InterPersonalRelationType.Id!.Value,
                DateRange = relation.DateRange,
                DocumentIdProof = relation.ProofDocument?.Id,
            });
        }
        IEnumerable<CreateModel.InterPersonalRelation> GetRelationsToInsert()
        {

            foreach (var relation in item.Where(x => !x.NodeId.HasValue)) {
                var now = DateTime.Now;
                yield return new CreateModel.InterPersonalRelation {
                    Id = null,
                    PublisherId = relation.PublisherId,
                    CreatedDateTime = now,
                    ChangedDateTime = now,
                    Title = relation.Title,
                    OwnerId = relation.OwnerId,
                    AuthoringStatusId = 1,
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
                    PersonIdFrom = relation.PersonFrom.Id!.Value,
                    PersonIdTo = relation.PersonTo.Id!.Value,
                    InterPersonalRelationTypeId = relation.InterPersonalRelationType.Id!.Value,
                    DateRange = relation.DateRange is null ? new DateTimeRange(null, null): relation.DateRange,
                    DocumentIdProof = relation.ProofDocument?.Id,
                };
            }
        }
        await _interPersonalRelationCreator.CreateAsync(GetRelationsToInsert().ToAsyncEnumerable(), connection);
    }
}