namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BlogPostCreator(
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<SimpleTextNode> simpleTextNodeInserterFactory,
        IDatabaseInserterFactory<BlogPost> blogPostInserterFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
    ) : EntityCreator<BlogPost>
{
    public override async Task CreateAsync(IAsyncEnumerable<BlogPost> blogPosts, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var simpleTextNodeWriter = await simpleTextNodeInserterFactory.CreateAsync(connection);
        await using var blogPostWriter = await blogPostInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

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
