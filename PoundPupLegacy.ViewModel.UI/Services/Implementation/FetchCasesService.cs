using Microsoft.Extensions.Logging;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;
internal sealed class FetchCasesService(
    IDbConnection connection,
    ILogger<FetchCasesService> logger,
    ISingleItemDatabaseReaderFactory<CasesDocumentReaderRequest, Cases> casesDocumentReaderFactory
) : DatabaseService(connection,logger), IFetchCasesService
{
    public async Task<Cases> FetchCases(int pageSize, int pageNumber, int tenantId, int userId)
    {
        var startIndex = (pageNumber - 1) * pageSize;

        return await WithConnection(async (connection) => {
            await using var reader = await casesDocumentReaderFactory.CreateAsync(connection);
            var cases = await reader.ReadAsync(new CasesDocumentReaderRequest {
                Limit = pageSize,
                Offset = startIndex,
                TenantId = tenantId,
                UserId = userId
            });
            var result = cases is not null
                ? cases
                : new Cases {
                    Entries = Array.Empty<NonSpecificCaseListEntry>(),
                    CaseTypes = Array.Empty<CaseTypeListEntry>(),
                    NumberOfEntries = 0,
                };

            return result;
        });
    }
}
