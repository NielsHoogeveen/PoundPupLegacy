namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class NodeFileInserterFactory : DatabaseInserterFactory<NodeFile>
{
    internal static NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    internal static NonNullableIntegerDatabaseParameter FileId = new() { Name = "file_id" };

    public override async Task<IDatabaseInserter<NodeFile>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "node_file",
            new DatabaseParameter[] {
                NodeId,
                FileId
            }
        );
        return new NodeFileInserter(command);
    }
}
internal sealed class NodeFileInserter : DatabaseInserter<NodeFile>
{
    internal NodeFileInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(NodeFile nodeFile)
    {
        Set(NodeFileInserterFactory.NodeId, nodeFile.NodeId);
        Set(NodeFileInserterFactory.FileId, nodeFile.FileId);
        await _command.ExecuteNonQueryAsync();
    }
}
