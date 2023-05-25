using PoundPupLegacy.CreateModel;
using PoundPupLegacy.CreateModel.Updaters;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal class PersonOrganizationRelationSaveService(
        IDatabaseUpdaterFactory<NodeUnpublishRequest> nodeUnpublishFactory,
        IDatabaseUpdaterFactory<ImmediatelyIdentifiablePersonOrganizationRelation> personOrganizationRelationUpdaterFactory,
        IEntityCreatorFactory<EventuallyIdentifiablePersonOrganizationRelation> personOrganizationRelationCreatorFactory
    ) : ISaveService<IEnumerable<PersonOrganizationRelation>>
{
    public async Task SaveAsync(IEnumerable<PersonOrganizationRelation> item, IDbConnection connection)
    {
        await using var unpublisher = await nodeUnpublishFactory.CreateAsync(connection);
        await using var updater = await personOrganizationRelationUpdaterFactory.CreateAsync(connection);

        foreach (var relation in item.OfType<ExistingPersonOrganizationRelation>().Where(x => x.HasBeenDeleted)) {
            await unpublisher.UpdateAsync(new NodeUnpublishRequest {
                NodeId = relation.NodeId
            });
        }
        foreach (var relation in item.OfType<ExistingPersonOrganizationRelation>().Where(x => !x.HasBeenDeleted)) {
            await updater.UpdateAsync(new CreateModel.ExistingPersonOrganizationRelation {
                Id = relation.NodeId,
                Title = relation.Title,
                PersonId = relation.Person.Id,
                OrganizationId = relation.Organization.Id,
                PersonOrganizationRelationTypeId = relation.PersonOrganizationRelationType.Id,
                DateRange = relation.DateRange is null ? new DateTimeRange(null, null) : relation.DateRange,
                DocumentIdProof = relation.ProofDocument?.Id,
                Description = relation.Description,
                GeographicalEntityId = relation.GeographicalEntity?.Id,
                AuthoringStatusId = 1,
                ChangedDateTime = DateTime.Now,
                NewNodeTerms = new List<NodeTerm>(),
                NewTenantNodes = new List<NewTenantNodeForExistingNode>(),
                NodeTermsToRemove = new List<NodeTerm>(),
                TenantNodesToRemove = new List<ExistingTenantNode>(),
                TenantNodesToUpdate = new List<ExistingTenantNode>(),
            });
        }
        IEnumerable<CreateModel.NewPersonOrganizationRelation> GetRelationsToInsert()
        {

            foreach (var relation in item.OfType<CompletedNewPersonOrganizationRelation>().Where(x => !x.HasBeenDeleted)) {
                var now = DateTime.Now;
                yield return new CreateModel.NewPersonOrganizationRelation {
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
                    PersonId = relation.Person.Id,
                    OrganizationId = relation.Organization.Id,
                    PersonOrganizationRelationTypeId = relation.PersonOrganizationRelationType.Id,
                    DateRange = relation.DateRange is null ? new DateTimeRange(null, null) : relation.DateRange,
                    DocumentIdProof = relation.ProofDocument?.Id,
                    GeographicalEntityId = relation.GeographicalEntity?.Id,
                    Description = relation.Description,
                    NodeTermIds = new List<int>(),
                };
            }
        }
        await using var personOrganizationRelationCreator = await personOrganizationRelationCreatorFactory.CreateAsync(connection);
        await personOrganizationRelationCreator.CreateAsync(GetRelationsToInsert().ToAsyncEnumerable());
    }
}