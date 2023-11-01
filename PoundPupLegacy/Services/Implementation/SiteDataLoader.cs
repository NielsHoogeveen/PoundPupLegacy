using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using PoundPupLegacy.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal sealed class SiteDataLoader(
    NpgsqlDataSource dataSource,
    ILogger<SiteDataService> logger,
    IConfiguration configuration,
    IMandatorySingleItemDatabaseReaderFactory<TenantReaderRequest, Tenant> tenantReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<UserDocumentReaderRequest, UserWithDetails> userDocumentReaderFactory
) : DatabaseService(dataSource, logger), ISiteDataLoader
{

    public async Task<SiteData> GetSiteData()
    {
        logger.LogInformation("Loading site data");
        var tenant = await LoadTenantAsync();
        var siteData =  new SiteData {
            Tenant = tenant,
            Users = new Dictionary<int, UserWithDetails> { {0, await LoadUser(tenant.Id, 0)} },
        };
        return siteData;
    }

    public async Task<UserWithDetails> LoadUser(int tenantId, int userId)
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
                foreach (var tenantNode in tenant.TenantNodes) {
                    tenant.UrlToId.Add(tenantNode.UrlPath, tenantNode.UrlId);
                    tenant.IdToUrl.Add(tenantNode.UrlId, tenantNode.UrlPath);
                }
                return tenant;
            }catch(Exception ex) {
                logger.LogError($"Unknown tenant id {tenantId} in appsettings.json");
                throw new LoadException();
            }
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
