namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class NodeFileInserter : DatabaseInserter<NodeFile>, IDatabaseInserter<NodeFile>
{
    private const string NODE_ID = "node_id";
    private const string FILE_ID = "file_id";
    public static async Task<DatabaseInserter<NodeFile>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
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

    internal override async Task WriteAsync(NodeFile nodeFile)
    {
        WriteValue(nodeFile.NodeId, NODE_ID);
        WriteValue(nodeFile.FileId, FILE_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
