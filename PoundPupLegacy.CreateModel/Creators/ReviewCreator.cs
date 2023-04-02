namespace PoundPupLegacy.CreateModel.Creators;

public class ReviewCreator : IEntityCreator<Review>
{
    public async Task CreateAsync(IAsyncEnumerable<Review> reviews, IDbConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var simpleTextNodeWriter = await SimpleTextNodeInserter.CreateAsync(connection);
        await using var reviewWriter = await ReviewInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

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
