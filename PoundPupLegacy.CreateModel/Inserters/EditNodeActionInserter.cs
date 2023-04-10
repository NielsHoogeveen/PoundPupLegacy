namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class EditNodeActionInserterFactory : DatabaseInserterFactory<EditNodeAction>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };

    public override async Task<IDatabaseInserter<EditNodeAction>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "edit_node_action",
            new DatabaseParameter[] {
                Id,
                NodeTypeId
            }
        );
        return new EditNodeActionInserter(command);
    }
}
internal sealed class EditNodeActionInserter : DatabaseInserter<EditNodeAction>
{
    internal EditNodeActionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(EditNodeAction editNodeAction)
    {
        if (!editNodeAction.Id.HasValue) {
            throw new NullReferenceException();
        }
        Set(EditNodeActionInserterFactory.Id, editNodeAction.Id.Value);
        Set(EditNodeActionInserterFactory.NodeTypeId, editNodeAction.NodeTypeId);
        await _command.ExecuteNonQueryAsync();
    }
}
