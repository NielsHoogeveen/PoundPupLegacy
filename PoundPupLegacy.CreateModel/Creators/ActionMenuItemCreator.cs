namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ActionMenuItemCreatorFactory(
    IDatabaseInserterFactory<MenuItem> menuItemInserterFactory,
    IDatabaseInserterFactory<ActionMenuItem> actionMenuItemInserterFactory
) : IEntityCreatorFactory<ActionMenuItem>
{
    public async Task<IEntityCreator<ActionMenuItem>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<ActionMenuItem>(new () {
            await menuItemInserterFactory.CreateAsync(connection),
            await actionMenuItemInserterFactory.CreateAsync(connection)
        });
}
