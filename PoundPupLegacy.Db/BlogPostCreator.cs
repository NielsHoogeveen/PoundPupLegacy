using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;

public class BlogPostCreator : IEntityCreator<BlogPost>
{
    public static async Task CreateAsync(IAsyncEnumerable<BlogPost> blogPosts, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var searchableWriter = await SearchableWriter.CreateAsync(connection);
        await using var simpleTextNodeWriter = await SimpleTextNodeWriter.CreateAsync(connection);
        await using var blogPostWriter = await BlogPostWriter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var blogPost in blogPosts)
        {
            await nodeWriter.WriteAsync(blogPost);
            await searchableWriter.WriteAsync(blogPost);
            await simpleTextNodeWriter.WriteAsync(blogPost);
            await blogPostWriter.WriteAsync(blogPost);
            foreach (var tenantNode in blogPost.TenantNodes)
            {
                tenantNode.NodeId = blogPost.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
