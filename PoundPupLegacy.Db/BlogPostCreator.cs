namespace PoundPupLegacy.Db;

public class BlogPostCreator : IEntityCreator<BlogPost>
{
    public static async Task CreateAsync(IEnumerable<BlogPost> blogPosts, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var blogPostWriter = await BlogPostWriter.CreateAsync(connection);

        foreach (var blogPost in blogPosts)
        {
            await nodeWriter.WriteAsync(blogPost);
            await blogPostWriter.WriteAsync(blogPost);
        }
    }
}
