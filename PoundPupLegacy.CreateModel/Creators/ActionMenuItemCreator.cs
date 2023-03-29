namespace PoundPupLegacy.CreateModel.Creators;

public class ActionMenuItemCreator : IEntityCreator<ActionMenuItem>
{
    public static async Task CreateAsync(IAsyncEnumerable<ActionMenuItem> actionMenuItems, NpgsqlConnection connection)
    {

        await using var menuItemWriter = await MenuItemInserter.CreateAsync(connection);
        await using var actionMenuItemWriter = await ActionMenuItemInserter.CreateAsync(connection);

        await foreach (var actionMenuItem in actionMenuItems) {
            await menuItemWriter.InsertAsync(actionMenuItem);
            await actionMenuItemWriter.InsertAsync(actionMenuItem);
        }

    }
}
