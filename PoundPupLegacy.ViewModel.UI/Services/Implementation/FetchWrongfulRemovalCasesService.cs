using Microsoft.Extensions.Logging;
using Npgsql;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;
internal sealed class FetchWrongfulRemovalCasesService(
    NpgsqlDataSource dataSource,
    ILogger<FetchWrongfulRemovalCasesService> logger,
    ISingleItemDatabaseReaderFactory<WrongfulRemovalCasesDocumentReaderRequest, WrongfulRemovalCases> abuseCasesDocumentReaderFactory
) : DatabaseService(dataSource, logger), IFetchWrongfulRemovalCasesService
{

    public async Task<WrongfulRemovalCases> FetchCases(int pageSize, int pageNumber, int tenantId, int userId, int[] selectedTerms)
    {
        var startIndex = (pageNumber - 1) * pageSize;

        return await WithConnection(async (connection) => {
            await using var reader = await abuseCasesDocumentReaderFactory.CreateAsync(connection);
            var cases = await reader.ReadAsync(new WrongfulRemovalCasesDocumentReaderRequest {
                Length = pageSize,
                StartIndex = startIndex,
                TenantId = tenantId,
                UserId = userId,
                SelectedTerms = selectedTerms
            });
            var result = cases is not null
                ? cases
                : new WrongfulRemovalCases {
                    TermNames = Array.Empty<SelectionItem>(),
                    Items = new WrongfulRemovalCaseList {
                        Entries = Array.Empty<CaseTeaserListEntry>(),
                        NumberOfEntries = 0,
                    }
                };

            return result;
        });
    }
}
