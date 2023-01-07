namespace PoundPupLegacy.Db;

public class DiscussionCreator : IEntityCreator<Discussion>
{
    public static async Task CreateAsync(IAsyncEnumerable<Discussion> discussions, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var simpleTextNodeWriter = await SimpleTextNodeWriter.CreateAsync(connection);
        await using var discussionWriter = await DiscussionWriter.CreateAsync(connection);

        await foreach (var discussion in discussions)
        {
            await nodeWriter.WriteAsync(discussion);
            await simpleTextNodeWriter.WriteAsync(discussion);
            await discussionWriter.WriteAsync(discussion);
        }
    }
}
