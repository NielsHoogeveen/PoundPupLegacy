using PoundPupLegacy.CreateModel.Creators;
using PoundPupLegacy.EditModel.Updaters;
using System.Data;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal class PersonOrganizationRelationSaveService : ISaveService<IEnumerable<PersonOrganizationRelation>>
{
    private readonly IDatabaseUpdaterFactory<NodeUnpublishRequest> _nodeUnpublishFactory;
    private readonly IDatabaseUpdaterFactory<PersonOrganizationRelationUpdaterRequest> _personOrganizationRelationUpdaterFactory;
    private readonly IEntityCreator<CreateModel.PersonOrganizationRelation> _personOrganizationRelationCreator;
    public PersonOrganizationRelationSaveService(
        IDatabaseUpdaterFactory<NodeUnpublishRequest> nodeUnpublishFactory,
        IDatabaseUpdaterFactory<PersonOrganizationRelationUpdaterRequest> personOrganizationRelationUpdaterFactory,
        IEntityCreator<CreateModel.PersonOrganizationRelation> personOrganizationRelationCreator
    )
    {
        _nodeUnpublishFactory = nodeUnpublishFactory;
        _personOrganizationRelationUpdaterFactory = personOrganizationRelationUpdaterFactory;
        _personOrganizationRelationCreator = personOrganizationRelationCreator;
    }
    public async Task SaveAsync(IEnumerable<PersonOrganizationRelation> item, IDbConnection connection)
    {
        await using var unpublisher = await _nodeUnpublishFactory.CreateAsync(connection);
        await using var updater = await _personOrganizationRelationUpdaterFactory.CreateAsync(connection);

        foreach (var relation in item.Where(x => x.HasBeenDeleted)) {
            if (!relation.NodeId.HasValue)
                throw new Exception("relation has no node id and cannot be unpublished");
            await unpublisher.UpdateAsync(new NodeUnpublishRequest {
                NodeId = relation.NodeId.Value
            });
        }
        foreach (var relation in item.Where(x => x.NodeId.HasValue && !x.HasBeenDeleted)) {
            await updater.UpdateAsync(new PersonOrganizationRelationUpdaterRequest {
                NodeId = relation.NodeId!.Value,
                Title = relation.Title,
                PersonId = relation.PersonId,
                OrganizationId = relation.OrganizationId,
                PersonOrganizationRelationTypeId = relation.PersonOrganizationRelationTypeId,
                DateRange = relation.DateRange is null ? new DateTimeRange(null, null): relation.DateRange,
                DocumentIdProof = relation.DocumentIdProof,
                Description = relation.Description,
                GeographicalEntityId = relation.GeographicalEntityId
            });
        }
        IEnumerable<CreateModel.PersonOrganizationRelation> GetRelationsToInsert()
        {

            foreach (var relation in item.Where(x => !x.NodeId.HasValue)) {
                var now = DateTime.Now;
                yield return new CreateModel.PersonOrganizationRelation {
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
                    PersonId = relation.PersonId,
                    OrganizationId = relation.OrganizationId,
                    PersonOrganizationRelationTypeId = relation.PersonOrganizationRelationTypeId,
                    DateRange = relation.DateRange is null ? new DateTimeRange(null, null): relation.DateRange,
                    DocumentIdProof = relation.DocumentIdProof,
                    GeographicalEntityId = relation.GeographicalEntityId,
                    Description = relation.Description,
                };
            }
        }
        await _personOrganizationRelationCreator.CreateAsync(GetRelationsToInsert().ToAsyncEnumerable(), connection);
    }
}