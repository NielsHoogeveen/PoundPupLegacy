using static PoundPupLegacy.EditModel.OrganizationPoliticalEntityRelation.CompletedOrganizationPoliticalEntityRelation;
using static PoundPupLegacy.EditModel.OrganizationPoliticalEntityRelation.CompletedOrganizationPoliticalEntityRelation.ResolvedOrganizationPoliticalEntityRelation;
using PoundPupLegacy.CreateModel.Creators;
using PoundPupLegacy.EditModel.Updaters;
using System.Data;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal class OrganizationPoliticalEntityRelationSaveService : ISaveService<IEnumerable<OrganizationPoliticalEntityRelation>>
{
    private readonly IDatabaseUpdaterFactory<NodeUnpublishRequest> _nodeUnpublishFactory;
    private readonly IDatabaseUpdaterFactory<PartyPoliticalEntityRelationUpdaterRequest> _partyPoliticalEntityRelationUpdaterFactory;
    private readonly IEntityCreator<CreateModel.PartyPoliticalEntityRelation> _partyPoliticalEntityRelationCreator;
    public OrganizationPoliticalEntityRelationSaveService(
        IDatabaseUpdaterFactory<NodeUnpublishRequest> nodeUnpublishFactory,
        IDatabaseUpdaterFactory<PartyPoliticalEntityRelationUpdaterRequest> partyPoliticalEntityRelationUpdaterFactory,
        IEntityCreator<CreateModel.PartyPoliticalEntityRelation> partyPoliticalEntityRelationCreator
    )
    {
        _nodeUnpublishFactory = nodeUnpublishFactory;
        _partyPoliticalEntityRelationUpdaterFactory = partyPoliticalEntityRelationUpdaterFactory;
        _partyPoliticalEntityRelationCreator = partyPoliticalEntityRelationCreator;
    }
    public async Task SaveAsync(IEnumerable<OrganizationPoliticalEntityRelation> item, IDbConnection connection)
    {
        await using var unpublisher = await _nodeUnpublishFactory.CreateAsync(connection);
        await using var updater = await _partyPoliticalEntityRelationUpdaterFactory.CreateAsync(connection);

        foreach (var relation in item.OfType<ExistingOrganizationPoliticalEntityRelation>().Where(x => x.HasBeenDeleted)) {
            await unpublisher.UpdateAsync(new NodeUnpublishRequest {
                NodeId = relation.NodeId
            });
        }
        foreach (var relation in item.OfType<ExistingOrganizationPoliticalEntityRelation>().Where(x => !x.HasBeenDeleted)) {
            await updater.UpdateAsync(new PartyPoliticalEntityRelationUpdaterRequest {
                NodeId = relation.NodeId,
                Title = relation.Title,
                PartyId = relation.Organization.Id,
                PoliticalEntityId = relation.PoliticalEntity.Id,
                PartyPoliticalEntityRelationTypeId = relation.OrganizationPoliticalEntityRelationType.Id,
                DateRange = relation.DateRange is null ? new DateTimeRange(null, null): relation.DateRange,
                DocumentIdProof = relation.ProofDocument?.Id
            });
        }
        IEnumerable<CreateModel.PartyPoliticalEntityRelation> GetRelationsToInsert()
        {

            foreach (var relation in item.OfType<CompletedNewOrganizationPoliticalEntityRelationExistingOrganization>().Where(x => !x.HasBeenDeleted)) {
                var now = DateTime.Now;
                yield return new CreateModel.PartyPoliticalEntityRelation {
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
                    PartyId = relation.Organization.Id,
                    PoliticalEntityId = relation.PoliticalEntity.Id,
                    PartyPoliticalEntityRelationTypeId = relation.OrganizationPoliticalEntityRelationType.Id,
                    DateRange = relation.DateRange is null ? new DateTimeRange(null, null): relation.DateRange,
                    DocumentIdProof = relation.ProofDocument?.Id,
                };
            }
        }
        await _partyPoliticalEntityRelationCreator.CreateAsync(GetRelationsToInsert().ToAsyncEnumerable(), connection);
    }
}