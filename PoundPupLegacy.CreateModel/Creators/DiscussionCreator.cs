namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DiscussionCreator : IEntityCreator<Discussion>
{
    public async Task CreateAsync(IAsyncEnumerable<Discussion> discussions, IDbConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var simpleTextNodeWriter = await SimpleTextNodeInserter.CreateAsync(connection);
        await using var discussionWriter = await DiscussionInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);


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
