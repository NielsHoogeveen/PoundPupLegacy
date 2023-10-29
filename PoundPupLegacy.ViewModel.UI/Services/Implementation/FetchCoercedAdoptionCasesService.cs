using Microsoft.Extensions.Logging;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;
internal sealed class FetchCoercedAdoptionCasesService(
    IDbConnection connection,
    ILogger<FetchCoercedAdoptionCasesService> logger,
    ISingleItemDatabaseReaderFactory<CoercedAdoptionCasesDocumentReaderRequest, CoercedAdoptionCases> abuseCasesDocumentReaderFactory
) : DatabaseService(connection,  logger), IFetchCoercedAdoptionCasesService
{
    public async Task<CoercedAdoptionCases> FetchCases(int pageSize, int pageNumber, int tenantId, int userId, int[] selectedTerms)
    {
        var startIndex = (pageNumber - 1) * pageSize;

        return await WithConnection(async (connection) => {
            await using var reader = await abuseCasesDocumentReaderFactory.CreateAsync(connection);
            var cases = await reader.ReadAsync(new CoercedAdoptionCasesDocumentReaderRequest {
                Length = pageSize,
                StartIndex = startIndex,
                TenantId = tenantId,
                UserId = userId,
                SelectedTerms = selectedTerms
            });
            var result = cases is not null
                ? cases
                : new CoercedAdoptionCases {
                    TermNames = Array.Empty<SelectionItem>(),
                    Items = new CoercedAdoptionCaseList {
                        Entries = Array.Empty<CaseTeaserListEntry>(),
                        NumberOfEntries = 0,
                    }
                };

            return result;
        });
    }
}
