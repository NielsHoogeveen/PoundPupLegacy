using Microsoft.Extensions.Logging;
using Npgsql;
using PoundPupLegacy.ViewModel.Readers;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;

internal sealed class FetchNameablesService(
    NpgsqlDataSource dataSource,
    ILogger<FetchNameablesService> logger,
    ISingleItemDatabaseReaderFactory<NameablesDocumentReaderRequest, Nameables> countriesDocumentReaderFactory
) : DatabaseService(dataSource, logger), IFetchNameablesService
{
    public async Task<Nameables?> FetchNameables(int tenantId, int userId, int nodeTypeId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await countriesDocumentReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NameablesDocumentReaderRequest { 
                TenantId = tenantId,
                UserId = userId,
                NodeTypeId = nodeTypeId
            });
        });
    }
}
