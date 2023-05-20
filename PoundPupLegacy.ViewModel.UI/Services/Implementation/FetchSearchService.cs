using Microsoft.Extensions.Logging;
using Npgsql;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;

internal sealed class FetchSearchService(
    IDbConnection connection,
    ILogger<FetchSearchService> logger,
    ISingleItemDatabaseReaderFactory<SearchDocumentReaderRequest, SearchResult> searchDocumentReaderFactory
) : DatabaseService(connection, logger), IFetchSearchService
{
    public async Task<SearchResult> FetchSearch(int userId, int tenantId, int pageSize, int pageNumber, string searchString)
    {
        int offset = (pageNumber - 1) * pageSize;
        return await WithConnection(async (connection) => {
            await using var reader = await searchDocumentReaderFactory.CreateAsync(connection);
            var searchResult = await reader.ReadAsync(new SearchDocumentReaderRequest {
                UserId = userId,
                TenantId = tenantId,
                Limit = pageSize,
                Offset = offset,
                SearchString = searchString
            });
            if (searchResult is not null)
                return searchResult;
            return new SearchResult {
                Entries = Array.Empty<SearchResultListEntry>(),
                NumberOfEntries = 0,
            };
        });
    }
}
