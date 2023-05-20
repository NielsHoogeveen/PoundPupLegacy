namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal class PersonPoliticalEntityRelationSaveService(
    IDatabaseUpdaterFactory<NodeUnpublishRequest> nodeUnpublishFactory,
    IDatabaseUpdaterFactory<PartyPoliticalEntityRelationUpdaterRequest> partyPoliticalEntityRelationUpdaterFactory,
    IEntityCreator<CreateModel.PartyPoliticalEntityRelation> partyPoliticalEntityRelationCreator
) : ISaveService<IEnumerable<PersonPoliticalEntityRelation>>
{
    public async Task SaveAsync(IEnumerable<PersonPoliticalEntityRelation> item, IDbConnection connection)
    {
        await using var unpublisher = await nodeUnpublishFactory.CreateAsync(connection);
        await using var updater = await partyPoliticalEntityRelationUpdaterFactory.CreateAsync(connection);

        foreach (var relation in item.OfType<ExistingPersonPoliticalEntityRelation>().Where(x => x.HasBeenDeleted)) {
            await unpublisher.UpdateAsync(new NodeUnpublishRequest {
                NodeId = relation.NodeId
            });
        }
        foreach (var relation in item.OfType<ExistingPersonPoliticalEntityRelation>().Where(x => !x.HasBeenDeleted)) {
            await updater.UpdateAsync(new PartyPoliticalEntityRelationUpdaterRequest {
                NodeId = relation.NodeId,
                Title = relation.Title,
                PartyId = relation.Person.Id,
                PoliticalEntityId = relation.PoliticalEntity.Id,
                PartyPoliticalEntityRelationTypeId = relation.PersonPoliticalEntityRelationType.Id,
                DateRange = relation.DateRange is null ? new DateTimeRange(null, null) : relation.DateRange,
                DocumentIdProof = relation.ProofDocument?.Id
            });
        }
        IEnumerable<CreateModel.PartyPoliticalEntityRelation> GetRelationsToInsert()
        {

            foreach (var relation in item.OfType<CompletedNewPersonPoliticalEntityRelationExistingPerson>().Where(x => !x.HasBeenDeleted)) {
                var now = DateTime.Now;
                yield return new CreateModel.PartyPoliticalEntityRelation {
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
                    PartyId = relation.Person.Id,
                    PoliticalEntityId = relation.PoliticalEntity.Id,
                    PartyPoliticalEntityRelationTypeId = relation.PersonPoliticalEntityRelationType.Id,
                    DateRange = relation.DateRange is null ? new DateTimeRange(null, null) : relation.DateRange,
                    DocumentIdProof = relation.ProofDocument?.Id,
                };
            }
        }
        await partyPoliticalEntityRelationCreator.CreateAsync(GetRelationsToInsert().ToAsyncEnumerable(), connection);
    }
}