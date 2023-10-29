using Microsoft.Extensions.Logging;
using Npgsql;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class TopicSearchService(
    NpgsqlDataSource dataSource,
    ILogger<TopicSearchService> logger,
    IDoesRecordExistDatabaseReaderFactory<TopicExistsRequest> doesTopcExistReaderFactory,
    IEnumerableDatabaseReaderFactory<TagDocumentsReaderRequest, NodeTerm.ForCreate> tagDocumentsReaderFactory
) : DatabaseService(dataSource, logger), ITopicSearchService
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
    public async Task<bool> DoesTopicExist(string name, int? id)
    {
        return await WithConnection(async connection => {
            var reader = await doesTopcExistReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new TopicExistsRequest { Name = name, TopicId = id });
        });
    }
}
