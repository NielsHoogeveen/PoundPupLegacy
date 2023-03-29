namespace PoundPupLegacy.CreateModel.Creators;

public class BlogPostCreator : IEntityCreator<BlogPost>
{
    public static async Task CreateAsync(IAsyncEnumerable<BlogPost> blogPosts, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var simpleTextNodeWriter = await SimpleTextNodeInserter.CreateAsync(connection);
        await using var blogPostWriter = await BlogPostInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var blogPost in blogPosts) {
            await nodeWriter.InsertAsync(blogPost);
            await searchableWriter.InsertAsync(blogPost);
            await simpleTextNodeWriter.InsertAsync(blogPost);
            await blogPostWriter.InsertAsync(blogPost);
            foreach (var tenantNode in blogPost.TenantNodes) {
                tenantNode.NodeId = blogPost.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
