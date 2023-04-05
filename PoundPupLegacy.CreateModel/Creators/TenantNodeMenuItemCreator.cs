namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class TenantNodeMenuItemCreator : EntityCreator<TenantNodeMenuItem>
{
    private readonly IDatabaseInserterFactory<MenuItem> _menuItemInserterFactory;
    private readonly IDatabaseInserterFactory<TenantNodeMenuItem> _tenantNodeMenuItemInserterFactory;
    public TenantNodeMenuItemCreator(
        IDatabaseInserterFactory<MenuItem> menuItemInserterFactory,
        IDatabaseInserterFactory<TenantNodeMenuItem> tenantNodeMenuItemInserterFactory
    )
    {
        _menuItemInserterFactory = menuItemInserterFactory;
        _tenantNodeMenuItemInserterFactory = tenantNodeMenuItemInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<TenantNodeMenuItem> tenantNodeMenuItems, IDbConnection connection)
    {
        await using var menuItemWriter = await _menuItemInserterFactory.CreateAsync(connection);
        await using var tenantNodeMenuItemWriter = await _tenantNodeMenuItemInserterFactory.CreateAsync(connection);

        await foreach (var tenantNodeMenuItem in tenantNodeMenuItems) {
            await menuItemWriter.InsertAsync(tenantNodeMenuItem);
            await tenantNodeMenuItemWriter.InsertAsync(tenantNodeMenuItem);
        }
    }
}
