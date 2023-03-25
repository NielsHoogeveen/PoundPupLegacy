using Npgsql;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal class FetchSearchService : IFetchSearchService
{
    private readonly NpgsqlConnection _connection;

    public FetchSearchService(
        NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<SearchResult> FetchSearch(int userId, int tenantId, int limit, int offset, string searchString)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await SearchDocumentReader.CreateAsync(_connection);
            return await reader.ReadAsync(userId, tenantId, limit, offset, searchString);

        }
        finally {
            if(_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }
}
