namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class NodeTypeInserter : DatabaseInserter<NodeType>, IDatabaseInserter<NodeType>
{

    private const string ID = "id";
    private const string NAME = "name";
    private const string DESCRIPTION = "description";
    private const string AUTHOR_SPECIFIC = "author_specific";
    public static async Task<DatabaseInserter<NodeType>> CreateAsync(NpgsqlConnection connection)
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
                new ColumnDefinition{
                    Name = AUTHOR_SPECIFIC,
                    NpgsqlDbType = NpgsqlDbType.Boolean
                },
            }
        );
        return new NodeTypeInserter(command);

    }

    internal NodeTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(NodeType nodeType)
    {
        WriteValue(nodeType.Id, ID);
        WriteNullableValue(nodeType.Name, NAME);
        WriteNullableValue(nodeType.Description, DESCRIPTION);
        WriteValue(nodeType.AuthorSpecific, AUTHOR_SPECIFIC);
        await _command.ExecuteNonQueryAsync();
    }
}
