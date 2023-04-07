using static NpgsqlTypes.NpgsqlTsQuery;

namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class NodeFileInserterFactory : DatabaseInserterFactory<NodeFile>
{
    public override async Task<IDatabaseInserter<NodeFile>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "node_file",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = NodeFileInserter.NODE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = NodeFileInserter.FILE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new NodeFileInserter(command);
    }
}
internal sealed class NodeFileInserter : DatabaseInserter<NodeFile>
{
    internal const string NODE_ID = "node_id";
    internal const string FILE_ID = "file_id";

    internal NodeFileInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(NodeFile nodeFile)
    {
        SetParameter(nodeFile.NodeId, NODE_ID);
        SetParameter(nodeFile.FileId, FILE_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
