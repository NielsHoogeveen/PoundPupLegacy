

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal class InterPersonalRelationFromSaveService(
    IDatabaseUpdaterFactory<NodeUnpublishRequest> nodeUnpublishFactory,
    IDatabaseUpdaterFactory<InterPersonalRelationUpdaterRequest> interPersonalRelationUpdaterFactory,
    IEntityCreator<CreateModel.NewInterPersonalRelation> interPersonalRelationCreator
) : ISaveService<IEnumerable<ResolvedInterPersonalRelationFrom>>
{
    public async Task SaveAsync(IEnumerable<ResolvedInterPersonalRelationFrom> item, IDbConnection connection)
    {
        await using var unpublisher = await nodeUnpublishFactory.CreateAsync(connection);
        await using var updater = await interPersonalRelationUpdaterFactory.CreateAsync(connection);

        foreach (var relation in item.OfType<ExistingInterPersonalRelationFrom>().Where(x => x.HasBeenDeleted)) {
            await unpublisher.UpdateAsync(new NodeUnpublishRequest {
                NodeId = relation.NodeId
            });
        }
        foreach (var relation in item.OfType<ExistingInterPersonalRelationFrom>().Where(x => !x.HasBeenDeleted)) {
            await updater.UpdateAsync(new InterPersonalRelationUpdaterRequest {
                NodeId = relation.NodeId,
                Title = relation.Title,
                Description = relation.Description,
                PersonIdFrom = relation.PersonFrom.Id,
                PersonIdTo = relation.PersonTo.Id,
                InterPersonalRelationTypeId = relation.InterPersonalRelationType.Id,
                DateRange = relation.DateRange,
                DocumentIdProof = relation.ProofDocument?.Id,
            });
        }
        IEnumerable<CreateModel.NewInterPersonalRelation> GetRelationsToInsert()
        {

            foreach (var relation in item.OfType<NewInterPersonalExistingRelationFrom>().Where(x => !x.HasBeenDeleted)) {
                var now = DateTime.Now;
                yield return new CreateModel.NewInterPersonalRelation {
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
                    PersonIdFrom = relation.PersonFrom.Id,
                    PersonIdTo = relation.PersonTo.Id,
                    InterPersonalRelationTypeId = relation.InterPersonalRelationType.Id,
                    DateRange = relation.DateRange is null ? new DateTimeRange(null, null) : relation.DateRange,
                    DocumentIdProof = relation.ProofDocument?.Id,
                    Description = relation.Description,
                };
            }
        }
        await interPersonalRelationCreator.CreateAsync(GetRelationsToInsert().ToAsyncEnumerable(), connection);
    }
}