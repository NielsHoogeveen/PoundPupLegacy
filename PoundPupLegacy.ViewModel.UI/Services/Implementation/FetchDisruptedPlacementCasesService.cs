using Microsoft.Extensions.Logging;
using Npgsql;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;
internal sealed class FetchDisruptedPlacementCasesService(
    NpgsqlDataSource dataSource,
    ILogger<FetchDisruptedPlacementCasesService> logger,
    ISingleItemDatabaseReaderFactory<DisruptedPlacementCasesDocumentReaderRequest, DisruptedPlacementCases> abuseCasesDocumentReaderFactory
): DatabaseService(dataSource, logger), IFetchDisruptedPlacementCasesService
{

    public async Task<DisruptedPlacementCases> FetchCases(int pageSize, int pageNumber, int tenantId, int userId, int[] selectedTerms)
    {
        var startIndex = (pageNumber - 1) * pageSize;

        return await WithConnection(async (connection) => {
            await using var reader = await abuseCasesDocumentReaderFactory.CreateAsync(connection);
            var cases = await reader.ReadAsync(new DisruptedPlacementCasesDocumentReaderRequest {
                Length = pageSize,
                StartIndex = startIndex,
                TenantId = tenantId,
                UserId = userId,
                SelectedTerms = selectedTerms
            });
            var result = cases is not null
                ? cases
                : new DisruptedPlacementCases {
                    TermNames = Array.Empty<SelectionItem>(),
                    Items = new DisruptedPlacementCaseList {
                        Entries = Array.Empty<CaseTeaserListEntry>(),
                        NumberOfEntries = 0,
                    }
                };

            return result;
        });
    }
}
