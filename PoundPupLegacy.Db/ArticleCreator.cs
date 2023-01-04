namespace PoundPupLegacy.Db;

public class ArticleCreator : IEntityCreator<Article>
{
    public static async Task CreateAsync(IEnumerable<Article> blogPosts, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var blogPostWriter = await ArticleWriter.CreateAsync(connection);

        foreach (var blogPost in blogPosts)
        {
            await nodeWriter.WriteAsync(blogPost);
            await blogPostWriter.WriteAsync(blogPost);
        }
    }
}
