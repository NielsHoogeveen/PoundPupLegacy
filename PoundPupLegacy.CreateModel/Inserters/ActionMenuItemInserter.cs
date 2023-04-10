namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class ActionMenuItemInserterFactory : DatabaseInserterFactory<ActionMenuItem>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableIntegerDatabaseParameter ActionId = new() { Name = "action_id" };

    public override async Task<IDatabaseInserter<ActionMenuItem>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "action_menu_item",
            new DatabaseParameter[] {
                Id,
                Name,
                ActionId
            }
        );
        return new ActionMenuItemInserter(command);
    }
}
internal sealed class ActionMenuItemInserter : DatabaseInserter<ActionMenuItem>
{
    internal ActionMenuItemInserter(NpgsqlCommand command) : base(command)
    {
    }
    public override async Task InsertAsync(ActionMenuItem actionMenuItem)
    {
        if (actionMenuItem.Id is null)
            throw new NullReferenceException();
        Set(ActionMenuItemInserterFactory.Id, actionMenuItem.Id.Value);
        Set(ActionMenuItemInserterFactory.Name, actionMenuItem.Name.Trim());
        Set(ActionMenuItemInserterFactory.ActionId, actionMenuItem.ActionId);
        await _command.ExecuteNonQueryAsync();
    }
}
