using Microsoft.Extensions.Logging;
using Npgsql;
using PoundPupLegacy.ViewModel.Readers;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;
internal sealed class FetchAbuseCasesMapService(
    NpgsqlDataSource dataSource,
    ILogger<FetchAbuseCasesService> logger,
    ISingleItemDatabaseReaderFactory<AbuseCasesMapDocumentReaderRequest, AbuseCaseMapEntry[]> abuseCasesMapDocumentReaderFactory
) : DatabaseService(dataSource, logger), IFetchAbuseCasesMapService
{
    public async Task<AbuseCaseMapEntry[]> FetchCasesMap(int tenantId, int userId)
    {

        return await WithConnection(async (connection) => {
            await using var reader = await abuseCasesMapDocumentReaderFactory.CreateAsync(connection);
            var cases = await reader.ReadAsync(new AbuseCasesMapDocumentReaderRequest {
                TenantId = tenantId,
                UserId = userId,
            });
            if(cases is null) {
                return Array.Empty<AbuseCaseMapEntry>();
            }
            return cases;
        });
    }
}
