namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ArticleCreator : EntityCreator<Article>
{
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<SimpleTextNode> _simpleTextNodeInserterFactory;
    private readonly IDatabaseInserterFactory<Article> _articleInserterFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    public ArticleCreator(
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<SimpleTextNode> simpleTextNodeInserterFactory,
        IDatabaseInserterFactory<Article> articleInserterFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
        )
    {
        _nodeInserterFactory = nodeInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _simpleTextNodeInserterFactory = simpleTextNodeInserterFactory;
        _articleInserterFactory = articleInserterFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<Article> articles, IDbConnection connection)
    {

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var simpleTextNodeWriter = await _simpleTextNodeInserterFactory.CreateAsync(connection);
        await using var articleWriter = await _articleInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var article in articles) {
            await nodeWriter.InsertAsync(article);
            await searchableWriter.InsertAsync(article);
            await simpleTextNodeWriter.InsertAsync(article);
            await articleWriter.InsertAsync(article);
            foreach (var tenantNode in article.TenantNodes) {
                tenantNode.NodeId = article.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
