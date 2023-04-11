namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ReviewCreator : EntityCreator<Review>
{
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<SimpleTextNode> _simpleTextNodeInserterFactory;
    private readonly IDatabaseInserterFactory<Review> _reviewInserterFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    public ReviewCreator(
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<SimpleTextNode> simpleTextNodeInserterFactory,
        IDatabaseInserterFactory<Review> reviewInserterFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
    )
    {
        _nodeInserterFactory = nodeInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _simpleTextNodeInserterFactory = simpleTextNodeInserterFactory;
        _reviewInserterFactory = reviewInserterFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<Review> reviews, IDbConnection connection)
    {
        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var simpleTextNodeWriter = await _simpleTextNodeInserterFactory.CreateAsync(connection);
        await using var reviewWriter = await _reviewInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var review in reviews) {
            await nodeWriter.InsertAsync(review);
            await searchableWriter.InsertAsync(review);
            await simpleTextNodeWriter.InsertAsync(review);
            await reviewWriter.InsertAsync(review);
            foreach (var tenantNode in review.TenantNodes) {
                tenantNode.NodeId = review.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
