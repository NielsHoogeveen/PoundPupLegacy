namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DiscussionCreator(
    IDatabaseInserterFactory<Discussion> discussionInserterFactory,
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<SimpleTextNode> simpleTextNodeInserterFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<Discussion>
{
    public override async Task CreateAsync(IAsyncEnumerable<Discussion> discussions, IDbConnection connection)
    {

        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var simpleTextNodeWriter = await simpleTextNodeInserterFactory.CreateAsync(connection);
        await using var discussionWriter = await discussionInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var discussion in discussions) {
            await nodeWriter.InsertAsync(discussion);
            await searchableWriter.InsertAsync(discussion);
            await simpleTextNodeWriter.InsertAsync(discussion);
            await discussionWriter.InsertAsync(discussion);
            foreach (var tenantNode in discussion.TenantNodes) {
                tenantNode.NodeId = discussion.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
