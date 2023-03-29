using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.ViewModel.Readers;
using SearchOption = PoundPupLegacy.ViewModel.SearchOption;

namespace PoundPupLegacy.Services.Implementation;

public class TopicService : ITopicService
{
    private readonly NpgsqlConnection _connection;
    private readonly IDatabaseReaderFactory<TopicsDocumentReader> _topicsDocumentReaderFactory;
    public TopicService(
        NpgsqlConnection connection,
        IDatabaseReaderFactory<TopicsDocumentReader> topicsDocumentReaderFactory)
    {
        _connection = connection;
        _topicsDocumentReaderFactory = topicsDocumentReaderFactory;
    }

    public async Task<Topics> FetchTopics(int userId, int tenantId, int limit, int offset, string searchTerm, SearchOption searchOption)
    {

        try {
            await _connection.OpenAsync();
            await using var reader = await _topicsDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(new TopicsDocumentReader.TopicsDocumentRequest {
                UserId = userId,
                TenantId = tenantId,
                Limit = limit,
                Offset = offset,
                SearchTerm = searchTerm,
                SearchOption = searchOption
            });
        }
        finally {
            await _connection.CloseAsync();
        }
    }
}
