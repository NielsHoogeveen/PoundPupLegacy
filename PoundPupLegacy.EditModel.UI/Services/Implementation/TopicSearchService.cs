namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class TopicSearchService : ITopicSearchService
{
    private readonly NpgsqlConnection _connection;

    private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
    private readonly IEnumerableDatabaseReaderFactory<TagDocumentsReaderRequest, Tag> _tagDocumentsReaderFactory;
    private readonly IDoesRecordExistDatabaseReaderFactory<TopicExistsRequest> _doesTopcExistReaderFactory;

    public TopicSearchService(
        IDbConnection connection,
        IDoesRecordExistDatabaseReaderFactory<TopicExistsRequest> doesTopcExistReaderFactory,
        IEnumerableDatabaseReaderFactory<TagDocumentsReaderRequest, Tag> tagDocumentsReaderFactory)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;
        _tagDocumentsReaderFactory = tagDocumentsReaderFactory;
        _doesTopcExistReaderFactory = doesTopcExistReaderFactory;
    }
    public async Task<List<Tag>> GetTerms(int? nodeId, int tenantId, string searchString, int[] nodeTypeIds)
    {
        await semaphore.WaitAsync();
        List<Tag> tags = new();
        try {
            await _connection.OpenAsync();
            await using var reader = await _tagDocumentsReaderFactory.CreateAsync(_connection);
            await foreach (var elem in reader.ReadAsync(new TagDocumentsReaderRequest {
                NodeId = nodeId,
                TenantId = tenantId,
                SearchString = searchString,
                NodeTypeIds = nodeTypeIds

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
    public async Task<bool> DoesTopicExist(string name)
    {
        var reader = await _doesTopcExistReaderFactory.CreateAsync(_connection);
        return await reader.ReadAsync(new TopicExistsRequest { Name = name });
    }
}
