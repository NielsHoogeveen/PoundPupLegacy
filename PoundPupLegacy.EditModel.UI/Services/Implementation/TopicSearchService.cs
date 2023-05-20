using Microsoft.Extensions.Logging;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class TopicSearchService(
    IDbConnection connection,
    ILogger<TopicSearchService> logger,
    IDoesRecordExistDatabaseReaderFactory<TopicExistsRequest> doesTopcExistReaderFactory,
    IEnumerableDatabaseReaderFactory<TagDocumentsReaderRequest, Tag> tagDocumentsReaderFactory
) : DatabaseService(connection, logger), ITopicSearchService
{

    private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
    public async Task<List<Tag>> GetTerms(int? nodeId, int tenantId, string searchString, int[] nodeTypeIds)
    {
        await semaphore.WaitAsync();
        List<Tag> tags = new();
        try {
            await connection.OpenAsync();
            await using var reader = await tagDocumentsReaderFactory.CreateAsync(connection);
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
            if (connection.State == ConnectionState.Open) {
                await connection.CloseAsync();
            }
            semaphore.Release();
        }
    }
    public async Task<bool> DoesTopicExist(string name)
    {
        var reader = await doesTopcExistReaderFactory.CreateAsync(connection);
        return await reader.ReadAsync(new TopicExistsRequest { Name = name });
    }
}
