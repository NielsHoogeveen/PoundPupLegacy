using Npgsql;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.ViewModel.Readers;
using SearchOption = PoundPupLegacy.ViewModel.SearchOption;

namespace PoundPupLegacy.Services.Implementation;

public class TopicService : ITopicService
{
    private readonly NpgsqlConnection _connection;

    public TopicService(
        NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<Topics> FetchTopics(int userId, int tenantId, int limit, int offset, string searchTerm, SearchOption searchOption)
    {
        
        try {
            await _connection.OpenAsync();
            await using var reader = await TopicsDocumentReader.CreateAsync(_connection);
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
