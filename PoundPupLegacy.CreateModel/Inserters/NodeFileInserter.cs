namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class NodeFileInserter : DatabaseInserter<NodeFile>, IDatabaseInserter<NodeFile>
{
    private const string NODE_ID = "node_id";
    private const string FILE_ID = "file_id";
    public static async Task<DatabaseInserter<NodeFile>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "node_file",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = NODE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = FILE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new NodeFileInserter(command);

    }

    internal NodeFileInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(NodeFile nodeFile)
    {
        WriteValue(nodeFile.NodeId, NODE_ID);
        WriteValue(nodeFile.FileId, FILE_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
