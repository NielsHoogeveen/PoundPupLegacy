namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ActionMenuItemCreator(
    IDatabaseInserterFactory<MenuItem> menuItemInserterFactory,
    IDatabaseInserterFactory<ActionMenuItem> actionMenuItemInserterFactory
) : EntityCreator<ActionMenuItem>
{
    public override async Task CreateAsync(IAsyncEnumerable<ActionMenuItem> actionMenuItems, IDbConnection connection)
    {
        await using var menuItemWriter = await menuItemInserterFactory.CreateAsync(connection);
        await using var actionMenuItemWriter = await actionMenuItemInserterFactory.CreateAsync(connection);

        await foreach (var actionMenuItem in actionMenuItems) {
            await menuItemWriter.InsertAsync(actionMenuItem);
            await actionMenuItemWriter.InsertAsync(actionMenuItem);
        }
    }
}
