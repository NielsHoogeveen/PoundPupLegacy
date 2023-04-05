namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BlogPostCreator : EntityCreator<BlogPost>
{
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<SimpleTextNode> _simpleTextNodeInserterFactory;
    private readonly IDatabaseInserterFactory<BlogPost> _blogPostInserterFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;

    public BlogPostCreator(
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<SimpleTextNode> simpleTextNodeInserterFactory,
        IDatabaseInserterFactory<BlogPost> blogPostInserterFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
        )
    {
        _nodeInserterFactory = nodeInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _simpleTextNodeInserterFactory = simpleTextNodeInserterFactory;
        _blogPostInserterFactory = blogPostInserterFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<BlogPost> blogPosts, IDbConnection connection)
    {

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var simpleTextNodeWriter = await _simpleTextNodeInserterFactory.CreateAsync(connection);
        await using var blogPostWriter = await _blogPostInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

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
