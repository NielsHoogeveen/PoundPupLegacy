namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ReviewCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<SimpleTextNode> simpleTextNodeInserterFactory,
    IDatabaseInserterFactory<NewReview> reviewInserterFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<NewReview>
{
    public override async Task CreateAsync(IAsyncEnumerable<NewReview> reviews, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var simpleTextNodeWriter = await simpleTextNodeInserterFactory.CreateAsync(connection);
        await using var reviewWriter = await reviewInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

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
