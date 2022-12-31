namespace PoundPupLegacy.Db;

public class DiscussionCreator : IEntityCreator<Discussion>
{
    public static void Create(IEnumerable<Discussion> blogPosts, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var blogPostWriter = DiscussionWriter.Create(connection);

        foreach (var blogPost in blogPosts)
        {
            nodeWriter.Write(blogPost);
            blogPostWriter.Write(blogPost);
        }
    }
}
