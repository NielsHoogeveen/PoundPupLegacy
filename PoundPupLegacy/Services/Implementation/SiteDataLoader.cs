using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using PoundPupLegacy.Readers;

namespace PoundPupLegacy.Services.Implementation;

internal sealed class SiteDataLoader(
    NpgsqlDataSource dataSource,
    ILogger<SiteDataService> logger,
    IConfiguration configuration,
    IMandatorySingleItemDatabaseReaderFactory<TenantReaderRequest, Tenant> tenantReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<UserDocumentReaderRequest, User> userDocumentReaderFactory,
    IEnumerableDatabaseReaderFactory<TenantNodesReaderRequest, TenantNode> tenantNodesReaderFactory
) : DatabaseService2(dataSource, logger), ISiteDataLoader
{

    public async Task<SiteData> GetSiteData()
    {
        logger.LogInformation("Loading site data");
        var tenant = await LoadTenantAsync();
        var siteData =  new SiteData {
            Tenant = tenant,
            Users = new Dictionary<int, User> { {0, await LoadUser(tenant.Id, 0)} },
        };
        return siteData;
    }

    public async Task<User> LoadUser(int tenantId, int userId)
    {
        
        try {
            return await WithConnection(async (connection) => {
                var userDocumentReader = await userDocumentReaderFactory.CreateAsync(connection);
                return await userDocumentReader.ReadAsync(new UserDocumentReaderRequest {
                    TenantId = tenantId,
                    UserId = userId
                });
            });
        }
        catch (ReadException re) {
            logger.LogError(re, "Error loading user");
            throw new LoadException();
        }
    }
    private async Task<Tenant> LoadTenantAsync(int tenantId)
    {
        return await WithConnection(async (connection) => {
            var tenantReader = await tenantReaderFactory.CreateAsync(connection);
            try {
                var tenant = await tenantReader.ReadAsync(new TenantReaderRequest { TenantId = tenantId });
                await tenantReader.DisposeAsync();
                await SetUrlPaths(tenant);
                return tenant;
            }catch(Exception ex) {
                logger.LogError($"Unknown tenant id {tenantId} in appsettings.json");
                throw new LoadException();
            }
        });
    }
    private async Task SetUrlPaths(Tenant tenant)
    {
        await WithConnection(async (connection) => {
            await using var tenantNodesReader = await tenantNodesReaderFactory.CreateAsync(connection);
            await foreach (var tenantNode in tenantNodesReader.ReadAsync(new TenantNodesReaderRequest { TenantId = tenant.Id })) {
                tenant.UrlToId.Add(tenantNode.UrlPath, tenantNode.UrlId);
                tenant.IdToUrl.Add(tenantNode.UrlId, tenantNode.UrlPath);
            }
            return Unit.Instance;
        });
    }

    private async Task<Tenant> LoadTenantAsync()
    {
        var tenantString = configuration["Tenant"];
        if (tenantString is null) {
            logger.LogError("Tenant is not defined in appsettings.json");
            throw new LoadException();
        }
        if (int.TryParse(tenantString, out int tenantId)) {
            var tenant = await LoadTenantAsync(tenantId);
            return tenant;
        }
        else {
            logger.LogError("tenant id in appsettings.json should be a number");
            throw new LoadException();
        }
    }
}
