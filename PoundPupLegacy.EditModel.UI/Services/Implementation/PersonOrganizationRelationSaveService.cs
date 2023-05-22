namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal class PersonOrganizationRelationSaveService(
        IDatabaseUpdaterFactory<NodeUnpublishRequest> nodeUnpublishFactory,
        IDatabaseUpdaterFactory<PersonOrganizationRelationUpdaterRequest> personOrganizationRelationUpdaterFactory,
        IEntityCreator<CreateModel.NewPersonOrganizationRelation> personOrganizationRelationCreator
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
            await updater.UpdateAsync(new PersonOrganizationRelationUpdaterRequest {
                NodeId = relation.NodeId,
                Title = relation.Title,
                PersonId = relation.Person.Id,
                OrganizationId = relation.Organization.Id,
                PersonOrganizationRelationTypeId = relation.PersonOrganizationRelationType.Id,
                DateRange = relation.DateRange is null ? new DateTimeRange(null, null) : relation.DateRange,
                DocumentIdProof = relation.ProofDocument?.Id,
                Description = relation.Description,
                GeographicalEntityId = relation.GeographicalEntity?.Id
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
                    PersonId = relation.Person.Id,
                    OrganizationId = relation.Organization.Id,
                    PersonOrganizationRelationTypeId = relation.PersonOrganizationRelationType.Id,
                    DateRange = relation.DateRange is null ? new DateTimeRange(null, null) : relation.DateRange,
                    DocumentIdProof = relation.ProofDocument?.Id,
                    GeographicalEntityId = relation.GeographicalEntity?.Id,
                    Description = relation.Description,
                };
            }
        }
        await personOrganizationRelationCreator.CreateAsync(GetRelationsToInsert().ToAsyncEnumerable(), connection);
    }
}