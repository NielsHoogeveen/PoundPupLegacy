using Npgsql;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;

internal sealed class FetchSearchService : IFetchSearchService
{
    private readonly NpgsqlConnection _connection;
    private readonly ISingleItemDatabaseReaderFactory<SearchDocumentReaderRequest, SearchResult> _searchDocumentReaderFactory;
    public FetchSearchService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<SearchDocumentReaderRequest, SearchResult> searchDocumentReaderFactory)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;

        _searchDocumentReaderFactory = searchDocumentReaderFactory;
    }

    public async Task<SearchResult> FetchSearch(int userId, int tenantId, int pageSize, int pageNumber, string searchString)
    {
        int offset = (pageNumber - 1) * pageSize;
        try
        {
            await _connection.OpenAsync();
            await using var reader = await _searchDocumentReaderFactory.CreateAsync(_connection);
            var searchResult = await reader.ReadAsync(new SearchDocumentReaderRequest
            {
                UserId = userId,
                TenantId = tenantId,
                Limit = pageSize,
                Offset = offset,
                SearchString = searchString
            });
            if (searchResult is not null)
                return searchResult;
            return new SearchResult
            {
                Entries = Array.Empty<SearchResultListEntry>(),
                NumberOfEntries = 0,
            };

        }
        finally
        {
            if (_connection.State == ConnectionState.Open)
            {
                await _connection.CloseAsync();
            }
        }
    }
}
