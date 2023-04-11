namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PageCreator : EntityCreator<Page>
{
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<SimpleTextNode> _simpleTextNodeInserterFactory;
    private readonly IDatabaseInserterFactory<Page> _pageInserterFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    public PageCreator(
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<SimpleTextNode> simpleTextNodeInserterFactory,
        IDatabaseInserterFactory<Page> pageInserterFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory)
    {
        _nodeInserterFactory = nodeInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _simpleTextNodeInserterFactory = simpleTextNodeInserterFactory;
        _pageInserterFactory = pageInserterFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<Page> pages, IDbConnection connection)
    {

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var simpleTextNodeWriter = await _simpleTextNodeInserterFactory.CreateAsync(connection);
        await using var pageWriter = await _pageInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var page in pages) {
            await nodeWriter.InsertAsync(page);
            await searchableWriter.InsertAsync(page);
            await simpleTextNodeWriter.InsertAsync(page);
            await pageWriter.InsertAsync(page);
            foreach (var tenantNode in page.TenantNodes) {
                tenantNode.NodeId = page.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
