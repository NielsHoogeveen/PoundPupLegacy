using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using PoundPupLegacy.Readers;
using System.Data;
using System.Diagnostics;

namespace PoundPupLegacy.Services.Implementation;

internal sealed class SiteDataLoader(
    IDbConnection connection,
    ILogger<SiteDataService> logger,
    IConfiguration configuration,
    IMandatorySingleItemDatabaseReaderFactory<TenantReaderRequest, Tenant> tenantReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<UserDocumentReaderRequest, User> userDocumentReaderFactory,
    IEnumerableDatabaseReaderFactory<TenantNodesReaderRequest, TenantNode> tenantNodesReaderFactory
) : DatabaseService(connection, logger), ISiteDataLoader
{

    public async Task<SiteData> GetSiteData()
    {
        _logger.LogInformation("Loading site data");
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
            _logger.LogError(re, "Error loading user");
            throw new LoadException();
        }
    }

    private async Task<Tenant> LoadTenantAsync()
    {
        var tenants = new List<Tenant>();
        var sw = Stopwatch.StartNew();
        var tenantString = configuration["Tenant"];
        if (tenantString is null) {
            _logger.LogError("Tenant is not defined in appsettings.json");
            throw new LoadException();
        }
        if (int.TryParse(tenantString, out int tenantId)) {
            return await WithConnection(async (connection) => {
                await using var tenantReader = await tenantReaderFactory.CreateAsync(connection);
                try {
                    var tenant = await tenantReader.ReadAsync(new TenantReaderRequest { TenantId = tenantId });
                    await using var tenantNodesReader = await tenantNodesReaderFactory.CreateAsync(connection);
                    await foreach (var tenantNode in tenantNodesReader.ReadAsync(new TenantNodesReaderRequest { TenantId = tenantId })) {
                        tenant.UrlToId.Add(tenantNode.UrlPath, tenantNode.UrlId);
                        tenant.IdToUrl.Add(tenantNode.UrlId, tenantNode.UrlPath);
                    }
                    _logger.LogInformation($"Loaded tenant urls in {sw.ElapsedMilliseconds}ms");
                    return tenant;
                }
                catch (ReadException) {
                    _logger.LogError("Unknown tenant id in appsettings.json");
                    throw new LoadException();
                }
            });
        }
        else {
            _logger.LogError("tenant id in appsettings.json should be a number");
            throw new LoadException();
        }
    }
}
