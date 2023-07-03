using Microsoft.Extensions.Logging;
using PoundPupLegacy.Admin.View;
using PoundPupLegacy.Admin.View.Readers;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.Admin.UI.Services.Implementation;

internal class TenantRetrieveService(
    IDbConnection connection,
    ILogger<TenantRetrieveService> logger,
    ISingleItemDatabaseReaderFactory<TenantReaderRequest, Tenant> tenantReaderFactory
    ) : DatabaseService(connection, logger), ITenantRetrieveService
{

    public async Task<Tenant?> GetTenant(int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var tenantReader = await tenantReaderFactory.CreateAsync(connection);
            return await tenantReader.ReadAsync(new TenantReaderRequest { TenantId = tenantId });
        });
    }
}
