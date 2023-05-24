namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ActionMenuItemCreatorFactory(
    IDatabaseInserterFactory<MenuItem> menuItemInserterFactory,
    IDatabaseInserterFactory<ActionMenuItem> actionMenuItemInserterFactory
) : IInsertingEntityCreatorFactory<ActionMenuItem>
{
    public async Task<InsertingEntityCreator<ActionMenuItem>> CreateAsync(IDbConnection connection) =>
        new (new () {
            await menuItemInserterFactory.CreateAsync(connection),
            await actionMenuItemInserterFactory.CreateAsync(connection)
        });
}
