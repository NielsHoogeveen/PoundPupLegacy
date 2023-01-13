using System.Runtime.CompilerServices;

namespace PoundPupLegacy.Db.Writers;

internal sealed class NodeTypeWriter : DatabaseWriter<NodeType>, IDatabaseWriter<NodeType>
{

    private const string ID = "id";
    private const string NAME = "name";
    private const string DESCRIPTION = "description";
    public static async Task<DatabaseWriter<NodeType>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "node_type",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = DESCRIPTION,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new NodeTypeWriter(command);

    }

    internal NodeTypeWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(NodeType nodeType)
    {
        WriteValue(nodeType.Id, ID);
        WriteNullableValue(nodeType.Name, NAME);
        WriteNullableValue(nodeType.Description, DESCRIPTION);
        await _command.ExecuteNonQueryAsync();
    }
}
