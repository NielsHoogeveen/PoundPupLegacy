using Microsoft.Extensions.Logging;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;
internal sealed class FetchFathersRightsViolationCasesService(
    IDbConnection connection,
    ILogger<FetchFathersRightsViolationCasesService> logger,
    ISingleItemDatabaseReaderFactory<FathersRightsViolationCasesDocumentReaderRequest, FathersRightsViolationCases> abuseCasesDocumentReaderFactory
) : DatabaseService(connection, logger), IFetchFathersRightsViolationCasesService
{

    public async Task<FathersRightsViolationCases> FetchCases(int pageSize, int pageNumber, int tenantId, int userId, int[] selectedTerms)
    {
        var startIndex = (pageNumber - 1) * pageSize;

        return await WithConnection(async (connection) => {
            await using var reader = await abuseCasesDocumentReaderFactory.CreateAsync(connection);
            var cases = await reader.ReadAsync(new FathersRightsViolationCasesDocumentReaderRequest {
                Length = pageSize,
                StartIndex = startIndex,
                TenantId = tenantId,
                UserId = userId,
                SelectedTerms = selectedTerms
            });
            var result = cases is not null
                ? cases
                : new FathersRightsViolationCases {
                    TermNames = Array.Empty<SelectionItem>(),
                    Items = new FathersRightsViolationCaseList {
                        Entries = Array.Empty<CaseTeaserListEntry>(),
                        NumberOfEntries = 0,
                    }
                };

            return result;
        });
    }
}
