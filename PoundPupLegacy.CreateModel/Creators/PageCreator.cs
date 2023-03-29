namespace PoundPupLegacy.CreateModel.Creators;

public class PageCreator : IEntityCreator<Page>
{
    public static async Task CreateAsync(IAsyncEnumerable<Page> pages, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var simpleTextNodeWriter = await SimpleTextNodeInserter.CreateAsync(connection);
        await using var pageWriter = await PageInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var page in pages) {
            await nodeWriter.WriteAsync(page);
            await searchableWriter.WriteAsync(page);
            await simpleTextNodeWriter.WriteAsync(page);
            await pageWriter.WriteAsync(page);
            foreach (var tenantNode in page.TenantNodes) {
                tenantNode.NodeId = page.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
