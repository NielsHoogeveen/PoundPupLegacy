namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class SimpleTextNodeWriter : DatabaseWriter<SimpleTextNode>, IDatabaseWriter<SimpleTextNode>
{
    private const string ID = "id";
    private const string TEXT = "text";
    private const string TEASER = "teaser";
    public static async Task<DatabaseWriter<SimpleTextNode>> CreateAsync(NpgsqlConnection connection)
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
        return new SimpleTextNodeWriter(command);

    }

    internal SimpleTextNodeWriter(NpgsqlCommand command) : base(command)
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
