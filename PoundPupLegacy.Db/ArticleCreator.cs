using System.Runtime.InteropServices;

namespace PoundPupLegacy.Db;

public class ArticleCreator : IEntityCreator<Article>
{
    public static async Task CreateAsync(IAsyncEnumerable<Article> blogPosts, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var simpleTextNodeWriter = await SimpleTextNodeWriter.CreateAsync(connection);
        await using var blogPostWriter = await ArticleWriter.CreateAsync(connection);

        await foreach (var blogPost in blogPosts)
        {
            await nodeWriter.WriteAsync(blogPost);
            await simpleTextNodeWriter.WriteAsync(blogPost);
            await blogPostWriter.WriteAsync(blogPost);
        }
    }
}
