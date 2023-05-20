namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PageCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<SimpleTextNode> simpleTextNodeInserterFactory,
    IDatabaseInserterFactory<Page> pageInserterFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<Page>
{
    public override async Task CreateAsync(IAsyncEnumerable<Page> pages, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var simpleTextNodeWriter = await simpleTextNodeInserterFactory.CreateAsync(connection);
        await using var pageWriter = await pageInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

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
