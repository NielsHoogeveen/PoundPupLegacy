using Npgsql;
using PoundPupLegacy.EditModel;
using PoundPupLegacy.EditModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

public class TopicSearchService : ITopicSearchService
{
    private readonly NpgsqlConnection _connection;

    private SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);

    public TopicSearchService(NpgsqlConnection connection)
    {
        _connection = connection;
    }
    public async Task<List<Tag>> GetTerms(int? nodeId, int tenantId, string str)
    {
        await semaphore.WaitAsync(TimeSpan.FromMilliseconds(100));
        List<Tag> tags = new();
        try {
            await _connection.OpenAsync();
            await using var reader = await TagDocumentsReader.CreateAsync(_connection);
            await foreach(var elem in reader.ReadAsync(nodeId, tenantId, str)) {
                tags.Add(elem);
            }
            return tags;
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
            semaphore.Release();
        }
    }
}
