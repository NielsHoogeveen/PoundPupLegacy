using Npgsql;
using PoundPupLegacy.Db.Writers;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;

public class ArticleCreator : IEntityCreator<Article>
{
    public static void Create(IEnumerable<Article> blogPosts, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var blogPostWriter = ArticleWriter.Create(connection);

        foreach (var blogPost in blogPosts)
        {
            nodeWriter.Write(blogPost);
            blogPostWriter.Write(blogPost);
        }
    }
}
