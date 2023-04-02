namespace PoundPupLegacy.CreateModel.Creators;

public class TenantNodeMenuItemCreator : IEntityCreator<TenantNodeMenuItem>
{
    public async Task CreateAsync(IAsyncEnumerable<TenantNodeMenuItem> tenantNodeMenuItems, IDbConnection connection)
    {

        await using var menuItemWriter = await MenuItemInserter.CreateAsync(connection);
        await using var tenantNodeMenuItemWriter = await TenantNodeMenuItemInserter.CreateAsync(connection);

        await foreach (var tenantNodeMenuItem in tenantNodeMenuItems) {
            await menuItemWriter.InsertAsync(tenantNodeMenuItem);
            await tenantNodeMenuItemWriter.InsertAsync(tenantNodeMenuItem);
        }

    }
}
