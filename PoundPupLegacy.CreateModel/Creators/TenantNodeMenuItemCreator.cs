namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class TenantNodeMenuItemCreator(
    IDatabaseInserterFactory<MenuItem> menuItemInserterFactory,
    IDatabaseInserterFactory<TenantNodeMenuItem> tenantNodeMenuItemInserterFactory
) : EntityCreator<TenantNodeMenuItem>
{
    public override async Task CreateAsync(IAsyncEnumerable<TenantNodeMenuItem> tenantNodeMenuItems, IDbConnection connection)
    {
        await using var menuItemWriter = await menuItemInserterFactory.CreateAsync(connection);
        await using var tenantNodeMenuItemWriter = await tenantNodeMenuItemInserterFactory.CreateAsync(connection);

        await foreach (var tenantNodeMenuItem in tenantNodeMenuItems) {
            await menuItemWriter.InsertAsync(tenantNodeMenuItem);
            await tenantNodeMenuItemWriter.InsertAsync(tenantNodeMenuItem);
        }
    }
}
