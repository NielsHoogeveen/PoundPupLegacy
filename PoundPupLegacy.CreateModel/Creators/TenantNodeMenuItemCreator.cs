namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class TenantNodeMenuItemCreatorFactory(
    IDatabaseInserterFactory<MenuItem> menuItemInserterFactory,
    IDatabaseInserterFactory<TenantNodeMenuItem> tenantNodeMenuItemInserterFactory
) : IInsertingEntityCreatorFactory<TenantNodeMenuItem>
{
    public async Task<InsertingEntityCreator<TenantNodeMenuItem>> CreateAsync(IDbConnection connection) => 
        new(new() {
            await tenantNodeMenuItemInserterFactory.CreateAsync(connection)
        });
}
