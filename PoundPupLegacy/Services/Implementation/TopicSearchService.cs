using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.EditModel;
using PoundPupLegacy.EditModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

public class TopicSearchService : ITopicSearchService
{
    private readonly NpgsqlConnection _connection;

    private SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
    private readonly IDatabaseReaderFactory<TagDocumentsReader> _tagDocumentsReaderFactory;

    public TopicSearchService(
        NpgsqlConnection connection,
        IDatabaseReaderFactory<TagDocumentsReader> tagDocumentsReaderFactory)
    {
        _connection = connection;
        _tagDocumentsReaderFactory = tagDocumentsReaderFactory;
    }
    public async Task<List<Tag>> GetTerms(int? nodeId, int tenantId, string str)
    {
        await semaphore.WaitAsync(TimeSpan.FromMilliseconds(100));
        List<Tag> tags = new();
        try {
            await _connection.OpenAsync();
            await using var reader = await _tagDocumentsReaderFactory.CreateAsync(_connection);
            await foreach (var elem in reader.ReadAsync(new TagDocumentsReader.TagDocumentsRequest {
                NodeId = nodeId,
                TenantId = tenantId,
                SearchString = str

            })) {
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
