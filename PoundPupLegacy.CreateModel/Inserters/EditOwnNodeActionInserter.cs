namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class EditOwnNodeActionInserterFactory : DatabaseInserterFactory<EditOwnNodeAction>
{
    public override async Task<IDatabaseInserter<EditOwnNodeAction>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "edit_own_node_action",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = EditOwnNodeActionInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = EditOwnNodeActionInserter.NODE_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new EditOwnNodeActionInserter(command);

    }
}
internal sealed class EditOwnNodeActionInserter : DatabaseInserter<EditOwnNodeAction>
{

    internal const string ID = "id";
    internal const string NODE_TYPE_ID = "node_type_id";


    internal EditOwnNodeActionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(EditOwnNodeAction editOwnNodeAction)
    {
        if (!editOwnNodeAction.Id.HasValue) {
            throw new NullReferenceException();
        }
        SetParameter(editOwnNodeAction.Id.Value, ID);
        SetNullableParameter(editOwnNodeAction.NodeTypeId, NODE_TYPE_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
