namespace PoundPupLegacy.CreateModel.Creators;

public class DiscussionCreator : IEntityCreator<Discussion>
{
    public static async Task CreateAsync(IAsyncEnumerable<Discussion> discussions, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var simpleTextNodeWriter = await SimpleTextNodeInserter.CreateAsync(connection);
        await using var discussionWriter = await DiscussionInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);


        await foreach (var discussion in discussions) {
            await nodeWriter.WriteAsync(discussion);
            await searchableWriter.WriteAsync(discussion);
            await simpleTextNodeWriter.WriteAsync(discussion);
            await discussionWriter.WriteAsync(discussion);
            foreach (var tenantNode in discussion.TenantNodes) {
                tenantNode.NodeId = discussion.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
