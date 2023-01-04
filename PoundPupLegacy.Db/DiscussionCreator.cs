namespace PoundPupLegacy.Db;

public class DiscussionCreator : IEntityCreator<Discussion>
{
    public static async Task CreateAsync(IAsyncEnumerable<Discussion> blogPosts, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var blogPostWriter = await DiscussionWriter.CreateAsync(connection);

        await foreach (var blogPost in blogPosts)
        {
            await nodeWriter.WriteAsync(blogPost);
            await blogPostWriter.WriteAsync(blogPost);
        }
    }
}
