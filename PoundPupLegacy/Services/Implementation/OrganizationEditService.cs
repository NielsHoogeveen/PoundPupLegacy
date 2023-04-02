using PoundPupLegacy.EditModel;
using PoundPupLegacy.EditModel.Readers;
using System.Data;
using PoundPupLegacy.Common;
using Npgsql;
using File = PoundPupLegacy.EditModel.File;

namespace PoundPupLegacy.Services.Implementation;

internal sealed class OrganizationEditService : PartyEditServiceBase<Organization, CreateModel.Organization>, IEditService<Organization>
{

    private readonly IDatabaseReaderFactory<OrganizationUpdateDocumentReader> _organizationUpdateDocumentReaderFactory;

    public OrganizationEditService(
        IDbConnection connection,
        ISiteDataService siteDataService,
        INodeCacheService nodeCacheService,
        IDatabaseReaderFactory<OrganizationUpdateDocumentReader> organizationUpdateDocumentReaderFactory,
        ISaveService<IEnumerable<Tag>> tagSaveService,
        ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
        ISaveService<IEnumerable<File>> filesSaveService,
        ITextService textService,
        ILogger<OrganizationEditService> logger

    ): base(
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
    }
    public async Task<Organization> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _organizationUpdateDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(new NodeEditDocumentReader.NodeUpdateDocumentRequest {
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

    public Task<Organization> GetViewModelAsync(int userId, int tenantId)
    {
        throw new NotImplementedException();
    }
    protected sealed override async Task StoreNew(Organization organization, NpgsqlConnection connection)
    {

    }

    protected sealed override async Task StoreExisting(Organization organization, NpgsqlConnection connection)
    {

    }

}
