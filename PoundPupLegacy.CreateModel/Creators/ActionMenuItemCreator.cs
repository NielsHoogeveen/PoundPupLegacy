namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ActionMenuItemCreator : EntityCreator<ActionMenuItem>
{
    private readonly IDatabaseInserterFactory<MenuItem> _menuItemInserterFactory;
    private readonly IDatabaseInserterFactory<ActionMenuItem> _actionMenuItemInserterFactory;
    public ActionMenuItemCreator(
        IDatabaseInserterFactory<MenuItem> menuItemInserterFactory,
        IDatabaseInserterFactory<ActionMenuItem> actionMenuItemInserterFactory
    )
    {
        _menuItemInserterFactory = menuItemInserterFactory;
        _actionMenuItemInserterFactory = actionMenuItemInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<ActionMenuItem> actionMenuItems, IDbConnection connection)
    {

        await using var menuItemWriter = await _menuItemInserterFactory.CreateAsync(connection);
        await using var actionMenuItemWriter = await _actionMenuItemInserterFactory.CreateAsync(connection);

        await foreach (var actionMenuItem in actionMenuItems) {
            await menuItemWriter.InsertAsync(actionMenuItem);
            await actionMenuItemWriter.InsertAsync(actionMenuItem);
        }

    }
}
