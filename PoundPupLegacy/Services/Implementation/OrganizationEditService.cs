using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.CreateModel.Creators;
using PoundPupLegacy.EditModel;
using PoundPupLegacy.EditModel.Readers;
using PoundPupLegacy.Updaters;
using System.Data;
using File = PoundPupLegacy.EditModel.File;

namespace PoundPupLegacy.Services.Implementation;

internal sealed class OrganizationEditService : PartyEditServiceBase<Organization, CreateModel.Organization>, IEditService<Organization>
{

    private readonly IDatabaseReaderFactory<OrganizationUpdateDocumentReader> _organizationUpdateDocumentReaderFactory;
    private readonly ISaveService<IEnumerable<Location>> _locationsSaveService;
    private readonly IDatabaseUpdaterFactory<OrganizationUpdater> _organizationUpdateFactory;
    private readonly IEntityCreator<CreateModel.Organization> _organizationEntityCreator;
    public OrganizationEditService(
        IDbConnection connection,
        ISiteDataService siteDataService,
        INodeCacheService nodeCacheService,
        IDatabaseReaderFactory<OrganizationUpdateDocumentReader> organizationUpdateDocumentReaderFactory,
        IDatabaseUpdaterFactory<OrganizationUpdater> organizationUpdateFactory,
        ISaveService<IEnumerable<Tag>> tagSaveService,
        ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
        ISaveService<IEnumerable<File>> filesSaveService,
        ISaveService<IEnumerable<Location>> locationsSaveService,
        IEntityCreator<CreateModel.Organization> organizationEntityCreator,
        ITextService textService,
        ILogger<OrganizationEditService> logger

    ) : base(
        connection,
        siteDataService,
        nodeCacheService,
        tagSaveService,
        tenantNodesSaveService,
        filesSaveService,
        textService,
        logger)
    {
        _organizationUpdateDocumentReaderFactory = organizationUpdateDocumentReaderFactory;
        _locationsSaveService = locationsSaveService;
        _organizationUpdateFactory = organizationUpdateFactory;
        _organizationEntityCreator = organizationEntityCreator;
    }
    public async Task<Organization> GetViewModelAsync(int urlId, int userId, int tenantId)
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
    public Task<Organization> GetViewModelAsync(int userId, int tenantId)
    {
        throw new NotImplementedException();
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

        await updater.UpdateAsync(new OrganizationUpdater.Request {
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
