using Npgsql;
using PoundPupLegacy.Db.Writers;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;

public class BlogPostCreator : IEntityCreator<BlogPost>
{
    public static void Create(IEnumerable<BlogPost> blogPosts, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var blogPostWriter = BlogPostWriter.Create(connection);

        foreach (var blogPost in blogPosts)
        {
            nodeWriter.Write(blogPost);
            blogPostWriter.Write(blogPost);
        }
    }
}
