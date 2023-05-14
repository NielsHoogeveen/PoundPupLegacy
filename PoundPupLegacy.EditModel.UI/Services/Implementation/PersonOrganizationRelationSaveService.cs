﻿using PoundPupLegacy.CreateModel.Creators;
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

        foreach (var relation in item.OfType<ExistingPersonOrganizationRelation>().Where(x => x.HasBeenDeleted)) {
            await unpublisher.UpdateAsync(new NodeUnpublishRequest {
                NodeId = relation.NodeId
            });
        }
        foreach (var relation in item.OfType<ExistingPersonOrganizationRelation>().Where(x => !x.HasBeenDeleted)) {
            await updater.UpdateAsync(new PersonOrganizationRelationUpdaterRequest {
                NodeId = relation.NodeId,
                Title = relation.Title,
                PersonId = relation.Person.Id!.Value,
                OrganizationId = relation.Organization.Id!.Value,
                PersonOrganizationRelationTypeId = relation.PersonOrganizationRelationType.Id!.Value,
                DateRange = relation.DateRange is null ? new DateTimeRange(null, null): relation.DateRange,
                DocumentIdProof = relation.ProofDocument?.Id,
                Description = relation.Description,
                GeographicalEntityId = relation.GeographicalEntity?.Id
            });
        }
        IEnumerable<CreateModel.PersonOrganizationRelation> GetRelationsToInsert()
        {

            foreach (var relation in item.OfType<CompletedNewPersonOrganizationRelation>()) {
                var now = DateTime.Now;
                yield return new CreateModel.PersonOrganizationRelation {
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
                    PersonId = relation.Person.Id,
                    OrganizationId = relation.Organization.Id!.Value,
                    PersonOrganizationRelationTypeId = relation.PersonOrganizationRelationType.Id!.Value,
                    DateRange = relation.DateRange is null ? new DateTimeRange(null, null): relation.DateRange,
                    DocumentIdProof = relation.ProofDocument?.Id,
                    GeographicalEntityId = relation.GeographicalEntity?.Id,
                    Description = relation.Description,
                };
            }
        }
        await _personOrganizationRelationCreator.CreateAsync(GetRelationsToInsert().ToAsyncEnumerable(), connection);
    }
}