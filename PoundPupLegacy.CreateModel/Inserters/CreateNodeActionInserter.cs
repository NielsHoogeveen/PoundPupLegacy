namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CreateNodeActionInserterFactory : DatabaseInserterFactory<CreateNodeAction>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };

    public override async Task<IDatabaseInserter<CreateNodeAction>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "create_node_action",
            new DatabaseParameter[] {
                Id,
                NodeTypeId
            }
        );
        return new CreateNodeActionInserter(command);
    }
}
internal sealed class CreateNodeActionInserter : DatabaseInserter<CreateNodeAction>
{

    internal CreateNodeActionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(CreateNodeAction createNodeAccessPrivilege)
    {
        if (!createNodeAccessPrivilege.Id.HasValue) {
            throw new NullReferenceException();
        }
        Set(CreateNodeActionInserterFactory.Id, createNodeAccessPrivilege.Id.Value);
        Set(CreateNodeActionInserterFactory.NodeTypeId, createNodeAccessPrivilege.NodeTypeId);
        await _command.ExecuteNonQueryAsync();
    }
}
