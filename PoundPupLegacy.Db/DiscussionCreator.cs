using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;

public class DiscussionCreator : IEntityCreator<Discussion>
{
    public static async Task CreateAsync(IAsyncEnumerable<Discussion> discussions, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var simpleTextNodeWriter = await SimpleTextNodeWriter.CreateAsync(connection);
        await using var discussionWriter = await DiscussionWriter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);


        await foreach (var discussion in discussions)
        {
            await nodeWriter.WriteAsync(discussion);
            await simpleTextNodeWriter.WriteAsync(discussion);
            await discussionWriter.WriteAsync(discussion);
            foreach (var tenantNode in discussion.TenantNodes)
            {
                tenantNode.NodeId = discussion.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
