using System.Xml.Linq;

namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class ActionMenuItemInserterFactory : DatabaseInserterFactory<ActionMenuItem>
{
    public override async Task<IDatabaseInserter<ActionMenuItem>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "action_menu_item",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ActionMenuItemInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = ActionMenuItemInserter.NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = ActionMenuItemInserter.ACTION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new ActionMenuItemInserter(command);

    }

}
internal sealed class ActionMenuItemInserter : DatabaseInserter<ActionMenuItem>
{

    internal const string ID = "id";
    internal const string NAME = "name";
    internal const string ACTION_ID = "action_id";

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
