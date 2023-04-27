using Npgsql;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;
using SearchOption = PoundPupLegacy.ViewModel.Models.SearchOption;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;

internal sealed class FetchTopicsService : IFetchTopicsService
{
    private readonly NpgsqlConnection _connection;
    private readonly ISingleItemDatabaseReaderFactory<TopicsDocumentReaderRequest, Topics> _topicsDocumentReaderFactory;
    public FetchTopicsService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<TopicsDocumentReaderRequest, Topics> topicsDocumentReaderFactory)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;

        _topicsDocumentReaderFactory = topicsDocumentReaderFactory;
    }

    public async Task<Topics> FetchTopics(int userId, int tenantId, int pageSize, int pageNumber, string searchTerm, SearchOption searchOption)
    {
        var offset = (pageNumber - 1) * pageSize;

        try {
            if (_connection.State == ConnectionState.Closed) {
                await _connection.OpenAsync();
            }
            await using var reader = await _topicsDocumentReaderFactory.CreateAsync(_connection);
            var result = await reader.ReadAsync(new TopicsDocumentReaderRequest {
                UserId = userId,
                TenantId = tenantId,
                Limit = pageSize,
                Offset = offset,
                SearchTerm = searchTerm,
                SearchOption = searchOption
            });
            if (result is not null)
                return result;
            else
                return new Topics {
                    Entries = Array.Empty<TopicListEntry>(),
                    NumberOfEntries = 0
                };
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }
}
