using Microsoft.Extensions.Logging;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class TopicSearchService(
    IDbConnection connection,
    ILogger<TopicSearchService> logger,
    IDoesRecordExistDatabaseReaderFactory<TopicExistsRequest> doesTopcExistReaderFactory,
    IEnumerableDatabaseReaderFactory<TagDocumentsReaderRequest, NodeTerm.ForCreate> tagDocumentsReaderFactory
) : DatabaseService(connection, logger), ITopicSearchService
{
    public async Task<List<NodeTerm.ForCreate>> GetTerms(int tenantId, string searchString, int[] nodeTypeIds)
    {
        List<NodeTerm.ForCreate> tags = new();
        return await WithSequencedConnection(async (connection) => {
            await using var reader = await tagDocumentsReaderFactory.CreateAsync(connection);
            await foreach (var elem in reader.ReadAsync(new TagDocumentsReaderRequest {
                TenantId = tenantId,
                SearchString = searchString,
                NodeTypeIds = nodeTypeIds

            })) {
                tags.Add(elem);
            }
            return tags;
        });
    }
    public async Task<bool> DoesTopicExist(string name)
    {
        var reader = await doesTopcExistReaderFactory.CreateAsync(connection);
        return await reader.ReadAsync(new TopicExistsRequest { Name = name });
    }
}
