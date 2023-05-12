using PoundPupLegacy.CreateModel.Creators;
using PoundPupLegacy.EditModel.Updaters;
using System.Data;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal class PartyPoliticalEntityRelationSaveService : ISaveService<IEnumerable<PartyPoliticalEntityRelation>>
{
    private readonly IDatabaseUpdaterFactory<NodeUnpublishRequest> _nodeUnpublishFactory;
    private readonly IDatabaseUpdaterFactory<PartyPoliticalEntityRelationUpdaterRequest> _partyPoliticalEntityRelationUpdaterFactory;
    private readonly IEntityCreator<CreateModel.PartyPoliticalEntityRelation> _partyPoliticalEntityRelationCreator;
    public PartyPoliticalEntityRelationSaveService(
        IDatabaseUpdaterFactory<NodeUnpublishRequest> nodeUnpublishFactory,
        IDatabaseUpdaterFactory<PartyPoliticalEntityRelationUpdaterRequest> partyPoliticalEntityRelationUpdaterFactory,
        IEntityCreator<CreateModel.PartyPoliticalEntityRelation> partyPoliticalEntityRelationCreator
    )
    {
        _nodeUnpublishFactory = nodeUnpublishFactory;
        _partyPoliticalEntityRelationUpdaterFactory = partyPoliticalEntityRelationUpdaterFactory;
        _partyPoliticalEntityRelationCreator = partyPoliticalEntityRelationCreator;
    }
    public async Task SaveAsync(IEnumerable<PartyPoliticalEntityRelation> item, IDbConnection connection)
    {
        await using var unpublisher = await _nodeUnpublishFactory.CreateAsync(connection);
        await using var updater = await _partyPoliticalEntityRelationUpdaterFactory.CreateAsync(connection);

        foreach (var relation in item.Where(x => x.HasBeenDeleted)) {
            if (!relation.NodeId.HasValue)
                throw new Exception("relation has no node id and cannot be unpublished");
            await unpublisher.UpdateAsync(new NodeUnpublishRequest {
                NodeId = relation.NodeId.Value
            });
        }
        foreach (var relation in item.Where(x => x.NodeId.HasValue && !x.HasBeenDeleted)) {
            await updater.UpdateAsync(new PartyPoliticalEntityRelationUpdaterRequest {
                NodeId = relation.NodeId!.Value,
                Title = relation.Title,
                PartyId = relation.Party.Id!.Value,
                PoliticalEntityId = relation.PoliticalEntity.Id!.Value,
                PartyPoliticalEntityRelationTypeId = relation.PartyPoliticalEntityRelationType.Id!.Value,
                DateRange = relation.DateRange is null ? new DateTimeRange(null, null): relation.DateRange,
                DocumentIdProof = relation.ProofDocument?.Id
            });
        }
        IEnumerable<CreateModel.PartyPoliticalEntityRelation> GetRelationsToInsert()
        {

            foreach (var relation in item.Where(x => !x.NodeId.HasValue)) {
                var now = DateTime.Now;
                yield return new CreateModel.PartyPoliticalEntityRelation {
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
                    PartyId = relation.Party.Id!.Value,
                    PoliticalEntityId = relation.PoliticalEntity.Id!.Value,
                    PartyPoliticalEntityRelationTypeId = relation.PartyPoliticalEntityRelationType.Id!.Value,
                    DateRange = relation.DateRange is null ? new DateTimeRange(null, null): relation.DateRange,
                    DocumentIdProof = relation.ProofDocument?.Id,
                };
            }
        }
        await _partyPoliticalEntityRelationCreator.CreateAsync(GetRelationsToInsert().ToAsyncEnumerable(), connection);
    }
}