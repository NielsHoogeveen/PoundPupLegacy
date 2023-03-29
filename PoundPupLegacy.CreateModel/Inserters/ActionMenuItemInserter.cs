namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class ActionMenuItemInserter : DatabaseInserter<ActionMenuItem>, IDatabaseInserter<ActionMenuItem>
{

    private const string ID = "id";
    private const string NAME = "name";
    private const string ACTION_ID = "action_id";
    public static async Task<DatabaseInserter<ActionMenuItem>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "action_menu_item",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = ACTION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new ActionMenuItemInserter(command);

    }

    internal ActionMenuItemInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(ActionMenuItem actionMenuItem)
    {
        WriteValue(actionMenuItem.Id, ID);
        WriteValue(actionMenuItem.Name.Trim(), NAME);
        WriteValue(actionMenuItem.ActionId, ACTION_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
