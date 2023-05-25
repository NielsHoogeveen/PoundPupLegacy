namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class TenantNodeMenuItemCreatorFactory(
    IDatabaseInserterFactory<MenuItem> menuItemInserterFactory,
    IDatabaseInserterFactory<TenantNodeMenuItem> tenantNodeMenuItemInserterFactory
) : IEntityCreatorFactory<TenantNodeMenuItem>
{
    public async Task<IEntityCreator<TenantNodeMenuItem>> CreateAsync(IDbConnection connection) => 
        new InsertingEntityCreator<TenantNodeMenuItem>(new() {
            await menuItemInserterFactory.CreateAsync(connection),
            await tenantNodeMenuItemInserterFactory.CreateAsync(connection)
        });
}
