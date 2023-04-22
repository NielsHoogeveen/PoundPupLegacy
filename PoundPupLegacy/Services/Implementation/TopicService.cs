using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;
using SearchOption = PoundPupLegacy.ViewModel.Models.SearchOption;

namespace PoundPupLegacy.Services.Implementation;

internal sealed class TopicService : ITopicService
{
    private readonly NpgsqlConnection _connection;
    private readonly ISingleItemDatabaseReaderFactory<TopicsDocumentReaderRequest, Topics> _topicsDocumentReaderFactory;
    public TopicService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<TopicsDocumentReaderRequest, Topics> topicsDocumentReaderFactory)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;

        _topicsDocumentReaderFactory = topicsDocumentReaderFactory;
    }

    public async Task<Topics?> FetchTopics(int userId, int tenantId, int pageSize, int pageNumber, string searchTerm, SearchOption searchOption)
    {
        var offset = (pageNumber - 1) * pageSize;

        try {
            if(_connection.State == ConnectionState.Closed) {
                await _connection.OpenAsync();
            }
            await using var reader = await _topicsDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(new TopicsDocumentReaderRequest {
                UserId = userId,
                TenantId = tenantId,
                Limit = pageSize,
                Offset = offset,
                SearchTerm = searchTerm,
                SearchOption = searchOption
            });
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }
}
