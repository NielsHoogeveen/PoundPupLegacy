namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class DeleteNodeActionInserterFactory : DatabaseInserterFactory<DeleteNodeAction>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };

    public override async Task<IDatabaseInserter<DeleteNodeAction>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "delete_node_action",
            new DatabaseParameter[] {
                Id,
                NodeTypeId
            }
        );
        return new DeleteNodeActionInserter(command);

    }

}
internal sealed class DeleteNodeActionInserter : DatabaseInserter<DeleteNodeAction>
{

    internal DeleteNodeActionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(DeleteNodeAction deleteNodeAccessPrivilege)
    {
        if (!deleteNodeAccessPrivilege.Id.HasValue) {
            throw new NullReferenceException();
        }
        Set(CreateNodeActionInserterFactory.Id, deleteNodeAccessPrivilege.Id.Value);
        Set(CreateNodeActionInserterFactory.NodeTypeId, deleteNodeAccessPrivilege.NodeTypeId);
        await _command.ExecuteNonQueryAsync();
    }
}
