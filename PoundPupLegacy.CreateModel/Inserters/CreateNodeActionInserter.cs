namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CreateNodeActionInserterFactory : DatabaseInserterFactory<CreateNodeAction>
{
    public override async Task<IDatabaseInserter<CreateNodeAction>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "create_node_action",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = CreateNodeActionInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = CreateNodeActionInserter.NODE_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new CreateNodeActionInserter(command);
    }
}
internal sealed class CreateNodeActionInserter : DatabaseInserter<CreateNodeAction>
{

    internal const string ID = "id";
    internal const string NODE_TYPE_ID = "node_type_id";

    internal CreateNodeActionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(CreateNodeAction createNodeAccessPrivilege)
    {
        if (!createNodeAccessPrivilege.Id.HasValue) {
            throw new NullReferenceException();
        }
        WriteValue(createNodeAccessPrivilege.Id.Value, ID);
        WriteNullableValue(createNodeAccessPrivilege.NodeTypeId, NODE_TYPE_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
