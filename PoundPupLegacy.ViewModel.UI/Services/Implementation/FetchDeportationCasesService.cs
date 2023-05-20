using Microsoft.Extensions.Logging;
using Npgsql;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;
internal sealed class FetchDeportationCasesService(
    IDbConnection connection,
    ILogger<FetchDeportationCasesService> logger,
    ISingleItemDatabaseReaderFactory<DeportationCasesDocumentReaderRequest, DeportationCases> abuseCasesDocumentReaderFactory
) : DatabaseService(connection, logger), IFetchDeportationCasesService
{
    public async Task<DeportationCases> FetchCases(int pageSize, int pageNumber, int tenantId, int userId, int[] selectedTerms)
    {
        var startIndex = (pageNumber - 1) * pageSize;

        return await WithConnection(async (connection) => {
            await using var reader = await abuseCasesDocumentReaderFactory.CreateAsync(connection);
            var cases = await reader.ReadAsync(new DeportationCasesDocumentReaderRequest {
                Length = pageSize,
                StartIndex = startIndex,
                TenantId = tenantId,
                UserId = userId,
                SelectedTerms = selectedTerms
            });
            var result = cases is not null
                ? cases
                : new DeportationCases {
                    TermNames = Array.Empty<SelectionItem>(),
                    Items = new DeportationCaseList {
                        Entries = Array.Empty<CaseListEntry>(),
                        NumberOfEntries = 0,
                    }
                };

            return result;
        });
    }
}
