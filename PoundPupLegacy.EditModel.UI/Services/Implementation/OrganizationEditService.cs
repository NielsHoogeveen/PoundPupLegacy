using Microsoft.Extensions.Logging;
using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class OrganizationEditService(
        IDbConnection connection,
        ILogger<OrganizationEditService> logger,
        ITenantRefreshService tenantRefreshService,
        ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewOrganization> organizationCreateDocumentReaderFactory,
        ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingOrganization> organizationUpdateDocumentReaderFactory,
        ISaveService<IEnumerable<ResolvedInterOrganizationalRelationFrom>> interOrganizationalRelationSaveServiceFrom,
        ISaveService<IEnumerable<ResolvedInterOrganizationalRelationTo>> interOrganizationalRelationSaveServiceTo,
        IDatabaseUpdaterFactory<OrganizationUpdaterRequest> organizationUpdateFactory,
        ISaveService<IEnumerable<Tag>> tagSaveService,
        ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
        ISaveService<IEnumerable<File>> filesSaveService,
        ISaveService<IEnumerable<Location>> locationsSaveService,
        INameableCreatorFactory<EventuallyIdentifiableOrganization> organizationEntityCreatorFactory,
        ITextService textService

    ) : PartyEditServiceBase<Organization, ExistingOrganization, NewOrganization, CreateModel.Organization>(
        connection,
        logger,
        tagSaveService,
        tenantNodesSaveService,
        filesSaveService,
        textService,
        tenantRefreshService
   ), IEditService<Organization, Organization>
{


    public async Task<Organization?> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await organizationUpdateDocumentReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeUpdateDocumentRequest {
                UrlId = urlId,
                UserId = userId,
                TenantId = tenantId
            });
        });
    }

    private async Task StoreInterOrganizationalRelationsFrom(Organization organization, int nodeId, NpgsqlConnection connection)
    {
        List<ResolvedInterOrganizationalRelationFrom> interOrganizationalRelation = organization
            .InterOrganizationalRelationsFrom
            .OfType<ExistingInterOrganizationalRelationFrom>()
            .OfType<ResolvedInterOrganizationalRelationFrom>()
            .ToList();
        IEnumerable<ResolvedInterOrganizationalRelationFrom> newToRelations = organization
                .InterOrganizationalRelationsFrom
                .OfType<CompletedNewInterOrganizationalNewFromRelation>()
                .Select(x => new NewInterOrganizationalExistingRelationFrom {
                    OrganizationFrom = new OrganizationListItem {
                        Id = nodeId,
                        Name = x.OrganizationFromName
                    },
                    OrganizationTo = x.OrganizationTo,
                    DateFrom = x.DateFrom,
                    DateTo = x.DateTo,
                    Description = x.Description,
                    Files = x.Files,
                    GeographicalEntity = x.GeographicalEntity,
                    HasBeenDeleted = x.HasBeenDeleted,
                    InterOrganizationalRelationType = x.InterOrganizationalRelationType,
                    MoneyInvolved = x.MoneyInvolved,
                    NodeTypeName = x.NodeTypeName,
                    NumberOfChildrenInvolved = x.NumberOfChildrenInvolved,
                    OwnerId = x.OwnerId,
                    PublisherId = x.PublisherId,
                    ProofDocument = x.ProofDocument,
                    Tags = x.Tags,
                    TenantNodes = x.TenantNodes,
                    Tenants = x.Tenants,
                    Title = x.Title,
                })
                .OfType<ResolvedInterOrganizationalRelationFrom>();
        IEnumerable<ResolvedInterOrganizationalRelationFrom> newFromRelations = organization
                .InterOrganizationalRelationsFrom
                .OfType<CompletedNewInterOrganizationalNewFromRelation>()
                .Select(x => new NewInterOrganizationalExistingRelationFrom {
                    OrganizationFrom = new OrganizationListItem {
                        Id = nodeId,
                        Name = x.OrganizationFromName
                    },
                    OrganizationTo = x.OrganizationTo,
                    DateFrom = x.DateFrom,
                    DateTo = x.DateTo,
                    Description = x.Description,
                    Files = x.Files,
                    GeographicalEntity = x.GeographicalEntity,
                    HasBeenDeleted = x.HasBeenDeleted,
                    InterOrganizationalRelationType = x.InterOrganizationalRelationType,
                    MoneyInvolved = x.MoneyInvolved,
                    NodeTypeName = x.NodeTypeName,
                    NumberOfChildrenInvolved = x.NumberOfChildrenInvolved,
                    OwnerId = x.OwnerId,
                    PublisherId = x.PublisherId,
                    ProofDocument = x.ProofDocument,
                    Tags = x.Tags,
                    TenantNodes = x.TenantNodes,
                    Tenants = x.Tenants,
                    Title = x.Title,

                })
                .OfType<ResolvedInterOrganizationalRelationFrom>();
        interOrganizationalRelation
            .AddRange(newToRelations);
        interOrganizationalRelation
            .AddRange(newFromRelations);
        await interOrganizationalRelationSaveServiceFrom.SaveAsync(interOrganizationalRelation, connection);
        await locationsSaveService.SaveAsync(organization.Locations, connection);
    }
    private async Task StoreInterOrganizationalRelationsTo(Organization organization, int nodeId, NpgsqlConnection connection)
    {
        List<ResolvedInterOrganizationalRelationTo> interOrganizationalRelation = organization
            .InterOrganizationalRelationsTo
            .OfType<ExistingInterOrganizationalRelationTo>()
            .OfType<ResolvedInterOrganizationalRelationTo>()
            .ToList();
        IEnumerable<ResolvedInterOrganizationalRelationTo> newToRelations = organization
                .InterOrganizationalRelationsTo
                .OfType<CompletedNewInterOrganizationalNewToRelation>()
                .Select(x => new NewInterOrganizationalExistingRelationTo {
                    OrganizationFrom = x.OrganizationFrom,
                    OrganizationTo = new OrganizationListItem {
                        Id = nodeId,
                        Name = x.OrganizationToName
                    },
                    DateFrom = x.DateFrom,
                    DateTo = x.DateTo,
                    Description = x.Description,
                    Files = x.Files,
                    GeographicalEntity = x.GeographicalEntity,
                    HasBeenDeleted = x.HasBeenDeleted,
                    InterOrganizationalRelationType = x.InterOrganizationalRelationType,
                    MoneyInvolved = x.MoneyInvolved,
                    NodeTypeName = x.NodeTypeName,
                    NumberOfChildrenInvolved = x.NumberOfChildrenInvolved,
                    OwnerId = x.OwnerId,
                    PublisherId = x.PublisherId,
                    ProofDocument = x.ProofDocument,
                    Tags = x.Tags,
                    TenantNodes = x.TenantNodes,
                    Tenants = x.Tenants,
                    Title = x.Title,
                })
                .OfType<ResolvedInterOrganizationalRelationTo>();
        IEnumerable<ResolvedInterOrganizationalRelationTo> newFromRelations = organization
                .InterOrganizationalRelationsTo
                .OfType<CompletedNewInterOrganizationalNewToRelation>()
                .Select(x => new NewInterOrganizationalExistingRelationTo {
                    OrganizationFrom = x.OrganizationFrom,
                    OrganizationTo = new OrganizationListItem {
                        Id = nodeId,
                        Name = x.OrganizationToName
                    },
                    DateFrom = x.DateFrom,
                    DateTo = x.DateTo,
                    Description = x.Description,
                    Files = x.Files,
                    GeographicalEntity = x.GeographicalEntity,
                    HasBeenDeleted = x.HasBeenDeleted,
                    InterOrganizationalRelationType = x.InterOrganizationalRelationType,
                    MoneyInvolved = x.MoneyInvolved,
                    NodeTypeName = x.NodeTypeName,
                    NumberOfChildrenInvolved = x.NumberOfChildrenInvolved,
                    OwnerId = x.OwnerId,
                    PublisherId = x.PublisherId,
                    ProofDocument = x.ProofDocument,
                    Tags = x.Tags,
                    TenantNodes = x.TenantNodes,
                    Tenants = x.Tenants,
                    Title = x.Title,

                })
                .OfType<ResolvedInterOrganizationalRelationTo>();
        interOrganizationalRelation
            .AddRange(newToRelations);
        interOrganizationalRelation
            .AddRange(newFromRelations);
        await interOrganizationalRelationSaveServiceTo.SaveAsync(interOrganizationalRelation, connection);
        await locationsSaveService.SaveAsync(organization.Locations, connection);
    }

    protected override async Task StoreAdditional(Organization organization, int nodeId, NpgsqlConnection connection)
    {
        await base.StoreAdditional(organization, nodeId, connection);
        await StoreInterOrganizationalRelationsFrom(organization, nodeId, connection);
        await StoreInterOrganizationalRelationsTo(organization, nodeId, connection);
    }
    public async Task<Organization?> GetViewModelAsync(int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await organizationCreateDocumentReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeCreateDocumentRequest {
                NodeTypeId = Constants.PERSON,
                UserId = userId,
                TenantId = tenantId
            });
        });
    }
    protected sealed override async Task<int> StoreNew(NewOrganization organization, NpgsqlConnection connection)
    {
        var now = DateTime.Now;
        var creationOrganization = new CreateModel.NewBasicOrganization {
            Id = null,
            Title = organization.Title,
            Description = organization.Description,
            EmailAddress = organization.EmailAddress,
            WebsiteUrl = organization.WebSiteUrl,
            Established = organization.Establishment,
            Terminated = organization.Termination,
            PublisherId = organization.PublisherId,
            CreatedDateTime = now,
            ChangedDateTime = now,
            FileIdTileImage = null,
            NodeTypeId = Constants.ORGANIZATION,
            OrganizationTypes = organization.OrganizationOrganizationTypes.Select(x => new CreateModel.OrganizationOrganizationType {
                OrganizationId = null,
                OrganizationTypeId = x.OrganizationTypeId
            }).ToList(),
            OwnerId = organization.OwnerId,
            AuthoringStatusId = 1,
            TenantNodes = organization.TenantNodes.Select(x => new CreateModel.NewTenantNodeForNewNode {
                NodeId = null,
                TenantId = x.TenantId,
                UrlPath = x.UrlPath,
                PublicationStatusId = x.PublicationStatusId,
                SubgroupId = x.SubgroupId,
                Id = null,
                UrlId = null
            }).ToList(),
            VocabularyNames = new List<CreateModel.VocabularyName>(),
            NodeTermIds = new List<int>(),
        };
        await using var organizationEntityCreator = await organizationEntityCreatorFactory.CreateAsync(connection);
        await organizationEntityCreator.CreateAsync(creationOrganization);
        return creationOrganization.Id!.Value;
    }

    protected sealed override async Task StoreExisting(ExistingOrganization organization, NpgsqlConnection connection)
    {
        var updater = await organizationUpdateFactory.CreateAsync(connection);

        await updater.UpdateAsync(new OrganizationUpdaterRequest {
            Title = organization.Title,
            Description = organization.Description,
            NodeId = organization.NodeId,
            EmailAddress = organization.EmailAddress,
            WebsiteUrl = organization.WebSiteUrl,
            EstablishmentDateRange = organization.Establishment?.ToDateTimeRange(),
            TerminationDateRange = organization.Establishment?.ToDateTimeRange(),
        });
    }

}
