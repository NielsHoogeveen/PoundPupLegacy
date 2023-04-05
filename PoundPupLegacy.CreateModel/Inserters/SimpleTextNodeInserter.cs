namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class SimpleTextNodeInserterFactory : DatabaseInserterFactory<SimpleTextNode>
{
    public override async Task<IDatabaseInserter<SimpleTextNode>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "simple_text_node",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = SimpleTextNodeInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = SimpleTextNodeInserter.TEXT,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = SimpleTextNodeInserter.TEASER,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new SimpleTextNodeInserter(command);
    }
}
internal sealed class SimpleTextNodeInserter : DatabaseInserter<SimpleTextNode>
{
    internal const string ID = "id";
    internal const string TEXT = "text";
    internal const string TEASER = "teaser";

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
