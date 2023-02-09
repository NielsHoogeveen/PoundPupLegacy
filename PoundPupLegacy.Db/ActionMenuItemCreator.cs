namespace PoundPupLegacy.Db;

public class ActionMenuItemCreator : IEntityCreator<ActionMenuItem>
{
    public static async Task CreateAsync(IAsyncEnumerable<ActionMenuItem> actionMenuItems, NpgsqlConnection connection)
    {

        await using var menuItemWriter = await MenuItemWriter.CreateAsync(connection);
        await using var actionMenuItemWriter = await ActionMenuItemWriter.CreateAsync(connection);

        await foreach (var actionMenuItem in actionMenuItems)
        {
            await menuItemWriter.WriteAsync(actionMenuItem);
            await actionMenuItemWriter.WriteAsync(actionMenuItem);
        }

    }
}
