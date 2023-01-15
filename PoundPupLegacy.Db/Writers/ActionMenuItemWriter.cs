namespace PoundPupLegacy.Db.Writers;

internal sealed class ActionMenuItemWriter : DatabaseWriter<ActionMenuItem>, IDatabaseWriter<ActionMenuItem>
{

    private const string ID = "id";
    private const string NAME = "name";
    private const string ACTION_ID = "action_id";
    public static async Task<DatabaseWriter<ActionMenuItem>> CreateAsync(NpgsqlConnection connection)
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
        return new ActionMenuItemWriter(command);

    }

    internal ActionMenuItemWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(ActionMenuItem actionMenuItem)
    {
        WriteValue(actionMenuItem.Id, ID);
        WriteValue(actionMenuItem.Name, NAME);
        WriteValue(actionMenuItem.ActionId, ACTION_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
