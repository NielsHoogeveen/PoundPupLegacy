namespace PoundPupLegacy.Db.Writers;

internal class NodeStatusWriter : DatabaseWriter<NodeStatus>, IDatabaseWriter<NodeStatus>
{
    private const string ID = "id";
    private const string NAME = "name";
    public static async Task<DatabaseWriter<NodeStatus>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "node_status",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new NodeStatusWriter(command);

    }

    internal NodeStatusWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(NodeStatus nodeStatus)
    {
        if (nodeStatus.Id is null)
            throw new NullReferenceException();

        WriteValue(nodeStatus.Id, ID);
        WriteValue(nodeStatus.Name, NAME);
        await _command.ExecuteNonQueryAsync();
    }
}
