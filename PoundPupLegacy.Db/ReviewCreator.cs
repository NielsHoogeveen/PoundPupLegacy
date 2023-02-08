using PoundPupLegacy.Model;
using System.Runtime.InteropServices;

namespace PoundPupLegacy.Db;

public class ReviewCreator : IEntityCreator<Review>
{
    public static async Task CreateAsync(IAsyncEnumerable<Review> reviews, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var searchableWriter = await SearchableWriter.CreateAsync(connection);
        await using var simpleTextNodeWriter = await SimpleTextNodeWriter.CreateAsync(connection);
        await using var reviewWriter = await ReviewWriter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var review in reviews)
        {
            await nodeWriter.WriteAsync(review);
            await searchableWriter.WriteAsync(review);
            await simpleTextNodeWriter.WriteAsync(review);
            await reviewWriter.WriteAsync(review);
            foreach (var tenantNode in review.TenantNodes)
            {
                tenantNode.NodeId = review.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
