//using PoundPupLegacy.CreateModel;
//using PoundPupLegacy.CreateModel.Deleters;
//using PoundPupLegacy.CreateModel.Updaters;

//namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

//internal class PersonPoliticalEntityRelationSaveService(
//    IDatabaseUpdaterFactory<NodeUnpublishRequest> nodeUnpublishFactory,
//    IDatabaseUpdaterFactory<ImmediatelyIdentifiablePartyPoliticalEntityRelation> partyPoliticalEntityRelationUpdaterFactory,
//    IEntityCreatorFactory<EventuallyIdentifiablePartyPoliticalEntityRelationForExistingParty> partyPoliticalEntityRelationCreatorFactory
//) : ISaveService<IEnumerable<PersonPoliticalEntityRelation>>
//{
//    public async Task SaveAsync(IEnumerable<PersonPoliticalEntityRelation> item, IDbConnection connection)
//    {
//        await using var unpublisher = await nodeUnpublishFactory.CreateAsync(connection);
//        await using var updater = await partyPoliticalEntityRelationUpdaterFactory.CreateAsync(connection);

//        foreach (var relation in item.OfType<ExistingPersonPoliticalEntityRelation>().Where(x => x.HasBeenDeleted)) {
//            await unpublisher.UpdateAsync(new NodeUnpublishRequest {
//                NodeId = relation.NodeId
//            });
//        }
//        foreach (var relation in item.OfType<ExistingPersonPoliticalEntityRelation>().Where(x => !x.HasBeenDeleted)) {
//            await updater.UpdateAsync(new CreateModel.ExistingPartyPoliticalEntityRelation {
//                Id = relation.NodeId,
//                Title = relation.Title,
//                PartyId = relation.Person.Id,
//                PoliticalEntityId = relation.PoliticalEntity.Id,
//                PartyPoliticalEntityRelationTypeId = relation.PersonPoliticalEntityRelationType.Id,
//                DateRange = relation.DateRange is null ? new DateTimeRange(null, null) : relation.DateRange,
//                DocumentIdProof = relation.ProofDocument?.Id,
//                AuthoringStatusId = 1,
//                ChangedDateTime = DateTime.Now,
//                NodeTermsToAdd = new List<NodeTermToAdd>(),
//                TenantNodesToAdd = new List<NewTenantNodeForExistingNode>(),
//                NodeTermsToRemove = new List<NodeTermToRemove>(),
//                TenantNodesToRemove = new List<TenantNodeToDelete>(),
//                TenantNodesToUpdate = new List<CreateModel.ExistingTenantNode>(),
//            });
//        }
//        IEnumerable<CreateModel.NewPartyPoliticalEntityRelationForExistingParty> GetRelationsToInsert()
//        {

//            foreach (var relation in item.OfType<CompletedNewPersonPoliticalEntityRelationExistingPerson>().Where(x => !x.HasBeenDeleted)) {
//                var now = DateTime.Now;
//                yield return new CreateModel.NewPartyPoliticalEntityRelationForExistingParty {
//                    Id = null,
//                    PublisherId = relation.PublisherId,
//                    CreatedDateTime = now,
//                    ChangedDateTime = now,
//                    Title = relation.Title,
//                    OwnerId = relation.OwnerId,
//                    AuthoringStatusId = 1,
//                    TenantNodes = relation.TenantNodes.Select(tenantNode => new CreateModel.NewTenantNodeForNewNode {
//                        Id = null,
//                        TenantId = Constants.PPL,
//                        PublicationStatusId = tenantNode.PublicationStatusId,
//                        UrlPath = tenantNode.UrlPath,
//                        SubgroupId = tenantNode.SubgroupId,
//                        UrlId = null
//                    }).ToList(),
//                    NodeTypeId = 47,
//                    PartyId = relation.Person.Id,
//                    PoliticalEntityId = relation.PoliticalEntity.Id,
//                    PartyPoliticalEntityRelationTypeId = relation.PersonPoliticalEntityRelationType.Id,
//                    DateRange = relation.DateRange is null ? new DateTimeRange(null, null) : relation.DateRange,
//                    DocumentIdProof = relation.ProofDocument?.Id,
//                    TermIds = new List<int>(),
//                };
//            }
//        }
//        await using var partyPoliticalEntityRelationCreator = await partyPoliticalEntityRelationCreatorFactory.CreateAsync(connection);
//        await partyPoliticalEntityRelationCreator.CreateAsync(GetRelationsToInsert().ToAsyncEnumerable());
//    }
//}