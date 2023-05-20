using Microsoft.Extensions.Logging;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;
internal sealed class FetchAbuseCasesService(
    IDbConnection connection,
    ILogger<FetchAbuseCasesService> logger,
    ISingleItemDatabaseReaderFactory<AbuseCasesDocumentReaderRequest, AbuseCases> abuseCasesDocumentReaderFactory
) : DatabaseService(connection, logger), IFetchAbuseCasesService
{
    public async Task<AbuseCases> FetchCases(int pageSize, int pageNumber, int tenantId, int userId, int[] selectedTerms)
    {
        var startIndex = (pageNumber - 1) * pageSize;

        return await WithConnection(async (connection) => {
            await using var reader = await abuseCasesDocumentReaderFactory.CreateAsync(connection);
            var cases = await reader.ReadAsync(new AbuseCasesDocumentReaderRequest {
                StartIndex = startIndex,
                Length = pageSize,
                TenantId = tenantId,
                UserId = userId,
                SelectedTerms = selectedTerms
            });
            var result = cases is not null
                ? cases
                : new AbuseCases {
                    Items = new AbuseCaseList {
                        Entries = Array.Empty<CaseListEntry>(),
                        NumberOfEntries = 0,
                    },
                    TermNames = Array.Empty<SelectionItem>()
                };
            return result;
        });
    }
}
