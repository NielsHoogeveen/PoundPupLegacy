using Microsoft.Extensions.Logging;
using Npgsql;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;
internal sealed class FetchAbuseCasesService(
    NpgsqlDataSource dataSource,
    ILogger<FetchAbuseCasesService> logger,
    ISingleItemDatabaseReaderFactory<AbuseCasesDocumentReaderRequest, AbuseCases> abuseCasesDocumentReaderFactory
) : DatabaseService(dataSource, logger), IFetchAbuseCasesService
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
                        Entries = Array.Empty<CaseTeaserListEntry>(),
                        NumberOfEntries = 0,
                    },
                    TermNames = Array.Empty<SelectionItem>()
                };
            return result;
        });
    }
}
