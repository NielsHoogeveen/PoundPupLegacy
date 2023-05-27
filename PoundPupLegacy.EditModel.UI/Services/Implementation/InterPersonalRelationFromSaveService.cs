

using PoundPupLegacy.CreateModel;
using PoundPupLegacy.CreateModel.Deleters;
using PoundPupLegacy.CreateModel.Updaters;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal class InterPersonalRelationFromSaveService(
    IDatabaseUpdaterFactory<NodeUnpublishRequest> nodeUnpublishFactory,
    IDatabaseUpdaterFactory<ImmediatelyIdentifiableInterPersonalRelation> interPersonalRelationUpdaterFactory,
    IEntityCreatorFactory<EventuallyIdentifiableInterPersonalRelation> interPersonalRelationCreatorFactory
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
            await updater.UpdateAsync(new ExistingInterPersonalRelation {
                Id = relation.NodeId,
                Title = relation.Title,
                Description = relation.Description,
                PersonIdFrom = relation.PersonFrom.Id,
                PersonIdTo = relation.PersonTo.Id,
                InterPersonalRelationTypeId = relation.InterPersonalRelationType.Id,
                DateRange = relation.DateRange,
                DocumentIdProof = relation.ProofDocument?.Id,
                AuthoringStatusId = 1,
                ChangedDateTime = DateTime.Now,
                NodeTermsToAdd = new List<NodeTermToAdd>(),
                TenantNodesToAdd = new List<NewTenantNodeForExistingNode>(),
                NodeTermsToRemove = new List<NodeTermToRemove>(),
                TenantNodesToRemove = new List<TenantNodeToDelete>(),
                TenantNodesToUpdate = new List<ExistingTenantNode>(),
            });
        }
        IEnumerable<CreateModel.NewInterPersonalRelationForExistingParticipants> GetRelationsToInsert()
        {

            foreach (var relation in item.OfType<NewInterPersonalExistingRelationFrom>().Where(x => !x.HasBeenDeleted)) {
                var now = DateTime.Now;
                yield return new CreateModel.NewInterPersonalRelationForExistingParticipants {
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
                    TermIds = new List<int>(),
                };
            }
        }
        await using var interPersonalRelationCreator = await interPersonalRelationCreatorFactory.CreateAsync(connection);
        await interPersonalRelationCreator.CreateAsync(GetRelationsToInsert().ToAsyncEnumerable());
    }
}