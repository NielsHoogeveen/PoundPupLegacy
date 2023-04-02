namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class SimpleTextNodeInserter : DatabaseInserter<SimpleTextNode>, IDatabaseInserter<SimpleTextNode>
{
    private const string ID = "id";
    private const string TEXT = "text";
    private const string TEASER = "teaser";
    public static async Task<DatabaseInserter<SimpleTextNode>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
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

    public override async Task InsertAsync(SimpleTextNode simpleTextNode)
    {
        if (simpleTextNode.Id is null)
            throw new NullReferenceException();

        WriteValue(simpleTextNode.Id, ID);
        WriteValue(simpleTextNode.Text, TEXT);
        WriteValue(simpleTextNode.Teaser, TEASER);
        await _command.ExecuteNonQueryAsync();
    }
}
