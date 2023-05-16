namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class OrganizationEditService : PartyEditServiceBase<Organization, ExistingOrganization, NewOrganization, CreateModel.Organization>, IEditService<Organization>
{


    private readonly ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewOrganization> _organizationCreateDocumentReaderFactory;
    private readonly ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingOrganization> _organizationUpdateDocumentReaderFactory;
    private readonly ISaveService<IEnumerable<ResolvedInterOrganizationalRelation>> _interOrganizationalRelationSaveService;
    private readonly ISaveService<IEnumerable<Location>> _locationsSaveService;
    private readonly IDatabaseUpdaterFactory<OrganizationUpdaterRequest> _organizationUpdateFactory;
    private readonly IEntityCreator<CreateModel.Organization> _organizationEntityCreator;
    public OrganizationEditService(
        IDbConnection connection,
        ITenantRefreshService tenantRefreshService,
        ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewOrganization> organizationCreateDocumentReaderFactory,
        ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingOrganization> organizationUpdateDocumentReaderFactory,
        ISaveService<IEnumerable<ResolvedInterOrganizationalRelation>> interOrganizationalRelationSaveService,
        IDatabaseUpdaterFactory<OrganizationUpdaterRequest> organizationUpdateFactory,
        ISaveService<IEnumerable<Tag>> tagSaveService,
        ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
        ISaveService<IEnumerable<File>> filesSaveService,
        ISaveService<IEnumerable<Location>> locationsSaveService,
        IEntityCreator<CreateModel.Organization> organizationEntityCreator,
        ITextService textService

    ) : base(
        connection,
        tagSaveService,
        tenantNodesSaveService,
        filesSaveService,
        textService,
        tenantRefreshService)
    {
        _organizationUpdateDocumentReaderFactory = organizationUpdateDocumentReaderFactory;
        _organizationCreateDocumentReaderFactory = organizationCreateDocumentReaderFactory;
        _interOrganizationalRelationSaveService = interOrganizationalRelationSaveService;
        _locationsSaveService = locationsSaveService;
        _organizationUpdateFactory = organizationUpdateFactory;
        _organizationEntityCreator = organizationEntityCreator;
    }
    public async Task<Organization?> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _organizationUpdateDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(new NodeUpdateDocumentRequest {
                UrlId = urlId,
                UserId = userId,
                TenantId = tenantId
            });
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }

    protected override async Task StoreAdditional(Organization organization, int nodeId)
    {
        await base.StoreAdditional(organization, nodeId);
        List<ResolvedInterOrganizationalRelation> interOrganizationalRelation = organization
            .InterOrganizationalRelations
            .OfType<ExistingInterOrganizationalRelation>()
            .OfType<ResolvedInterOrganizationalRelation>()
            .ToList();
        IEnumerable<ResolvedInterOrganizationalRelation> newToRelations = organization
                .InterOrganizationalRelations
                .OfType<CompletedNewInterOrganizationalNewToRelation>()
                .Select(x => new NewInterOrganizationalExistingRelation {
                    OrganizationFrom = x.OrganizationFrom,
                    OrganizationTo = new OrganizationListItem {
                        Id = nodeId,
                        Name = x.OrganizationToName
                    },
                    SettableRelationSideThisOrganization = RelationSide.To,
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
                .OfType<ResolvedInterOrganizationalRelation>();
        IEnumerable<ResolvedInterOrganizationalRelation> newFromRelations = organization
                .InterOrganizationalRelations
                .OfType<CompletedNewInterOrganizationalNewFromRelation>()
                .Select(x => new NewInterOrganizationalExistingRelation {
                    OrganizationFrom = new OrganizationListItem {
                        Id = nodeId,
                        Name = x.OrganizationFromName
                    },
                    SettableRelationSideThisOrganization = RelationSide.From,
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
                .OfType<ResolvedInterOrganizationalRelation>();
        interOrganizationalRelation
            .AddRange(newToRelations);
        interOrganizationalRelation
            .AddRange(newFromRelations);
        await _interOrganizationalRelationSaveService.SaveAsync(interOrganizationalRelation, _connection);
        await _locationsSaveService.SaveAsync(organization.Locations, _connection);
    }
    public async Task<Organization?> GetViewModelAsync(int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _organizationCreateDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(new NodeCreateDocumentRequest {
                NodeTypeId = Constants.PERSON,
                UserId = userId,
                TenantId = tenantId
            });
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }
    protected sealed override async Task<int> StoreNew(NewOrganization organization, NpgsqlConnection connection)
    {
        var now = DateTime.Now;
        var creationOrganization = new CreateModel.BasicOrganization {
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
            TenantNodes = organization.TenantNodes.Select(x => new CreateModel.TenantNode {
                NodeId = null,
                TenantId = x.TenantId,
                UrlPath = x.UrlPath,
                PublicationStatusId = x.PublicationStatusId,
                SubgroupId = x.SubgroupId,
                Id = null,
                UrlId = null
            }).ToList(),
            VocabularyNames = new List<CreateModel.VocabularyName>()
        };
        await _organizationEntityCreator.CreateAsync(creationOrganization, connection);
        return creationOrganization.Id!.Value;
    }

    protected sealed override async Task StoreExisting(ExistingOrganization organization, NpgsqlConnection connection)
    {
        var updater = await _organizationUpdateFactory.CreateAsync(connection);

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
