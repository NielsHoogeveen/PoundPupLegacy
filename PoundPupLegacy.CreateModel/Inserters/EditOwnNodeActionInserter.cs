namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class EditOwnNodeActionInserterFactory : DatabaseInserterFactory<EditOwnNodeAction>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };

    public override async Task<IDatabaseInserter<EditOwnNodeAction>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "edit_own_node_action",
            new DatabaseParameter[] {
                Id,
                NodeTypeId
            }
        );
        return new EditOwnNodeActionInserter(command);

    }
}
internal sealed class EditOwnNodeActionInserter : DatabaseInserter<EditOwnNodeAction>
{
    internal EditOwnNodeActionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(EditOwnNodeAction editOwnNodeAction)
    {
        if (!editOwnNodeAction.Id.HasValue) {
            throw new NullReferenceException();
        }
        Set(EditNodeActionInserterFactory.Id, editOwnNodeAction.Id.Value);
        Set(EditNodeActionInserterFactory.NodeTypeId, editOwnNodeAction.NodeTypeId);
        await _command.ExecuteNonQueryAsync();
    }
}
