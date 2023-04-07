namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class DeleteNodeActionInserterFactory : DatabaseInserterFactory<DeleteNodeAction>
{
    public override async Task<IDatabaseInserter<DeleteNodeAction>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "delete_node_action",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = DeleteNodeActionInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = DeleteNodeActionInserter.NODE_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new DeleteNodeActionInserter(command);

    }

}
internal sealed class DeleteNodeActionInserter : DatabaseInserter<DeleteNodeAction>
{

    internal const string ID = "id";
    internal const string NODE_TYPE_ID = "node_type_id";

    internal DeleteNodeActionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(DeleteNodeAction deleteNodeAccessPrivilege)
    {
        if (!deleteNodeAccessPrivilege.Id.HasValue) {
            throw new NullReferenceException();
        }
        SetParameter(deleteNodeAccessPrivilege.Id.Value, ID);
        SetNullableParameter(deleteNodeAccessPrivilege.NodeTypeId, NODE_TYPE_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
