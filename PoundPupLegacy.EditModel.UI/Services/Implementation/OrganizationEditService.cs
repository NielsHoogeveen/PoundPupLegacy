using Npgsql;
using PoundPupLegacy.CreateModel.Creators;
using PoundPupLegacy.EditModel.Readers;
using PoundPupLegacy.EditModel.Updaters;
using System.Data;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class OrganizationEditService : PartyEditServiceBase<Organization, CreateModel.Organization>, IEditService<Organization>
{

    private readonly ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, Organization> _organizationUpdateDocumentReaderFactory;
    private readonly ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, Organization> _organizationCreateDocumentReaderFactory;
    private readonly ISaveService<IEnumerable<Location>> _locationsSaveService;
    private readonly IDatabaseUpdaterFactory<OrganizationUpdaterRequest> _organizationUpdateFactory;
    private readonly IEntityCreator<CreateModel.Organization> _organizationEntityCreator;
    public OrganizationEditService(
        IDbConnection connection,
        ITenantRefreshService tenantRefreshService,
        ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, Organization> organizationUpdateDocumentReaderFactory,
        ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, Organization> organizationCreateDocumentReaderFactory,
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

    protected override async Task StoreAdditional(Organization organization)
    {
        await base.StoreAdditional(organization);
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
    protected sealed override async Task StoreNew(Organization organization, NpgsqlConnection connection)
    {
        var now = DateTime.Now;
        await _organizationEntityCreator.CreateAsync(new CreateModel.BasicOrganization {
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
        }
        , connection
        );
    }

    protected sealed override async Task StoreExisting(Organization organization, NpgsqlConnection connection)
    {
        if (!organization.NodeId.HasValue) {
            throw new Exception("NodeId of organization should have a value");
        }
        var updater = await _organizationUpdateFactory.CreateAsync(connection);

        await updater.UpdateAsync(new OrganizationUpdaterRequest {
            Title = organization.Title,
            Description = organization.Description,
            NodeId = organization.NodeId.Value,
            EmailAddress = organization.EmailAddress,
            WebsiteUrl = organization.WebSiteUrl,
            EstablishmentDateRange = organization.Establishment?.ToDateTimeRange(),
            TerminationDateRange = organization.Establishment?.ToDateTimeRange(),
        });
    }

}
