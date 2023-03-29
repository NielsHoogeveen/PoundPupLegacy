namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class SimpleTextNodeInserter : DatabaseInserter<SimpleTextNode>, IDatabaseInserter<SimpleTextNode>
{
    private const string ID = "id";
    private const string TEXT = "text";
    private const string TEASER = "teaser";
    public static async Task<DatabaseInserter<SimpleTextNode>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "simple_text_node",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = TEXT,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = TEASER,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new SimpleTextNodeInserter(command);

    }

    internal SimpleTextNodeInserter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(SimpleTextNode simpleTextNode)
    {
        if (simpleTextNode.Id is null)
            throw new NullReferenceException();

        WriteValue(simpleTextNode.Id, ID);
        WriteValue(simpleTextNode.Text, TEXT);
        WriteValue(simpleTextNode.Teaser, TEASER);
        await _command.ExecuteNonQueryAsync();
    }
}
