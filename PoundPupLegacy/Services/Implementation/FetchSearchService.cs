using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal class FetchSearchService : IFetchSearchService
{
    private readonly NpgsqlConnection _connection;
    private readonly IDatabaseReaderFactory<SearchDocumentReader> _searchDocumentReaderFactory;
    public FetchSearchService(
        NpgsqlConnection connection,
        IDatabaseReaderFactory<SearchDocumentReader> searchDocumentReaderFactory)
    {
        _connection = connection;
        _searchDocumentReaderFactory = searchDocumentReaderFactory;
    }

    public async Task<SearchResult> FetchSearch(int userId, int tenantId, int limit, int offset, string searchString)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _searchDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(new SearchDocumentReader.SearchDocumentRequest {
                UserId = userId,
                TenantId = tenantId,
                Limit = limit,
                Offset = offset,
                SearchString = searchString
            });

        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }
}
