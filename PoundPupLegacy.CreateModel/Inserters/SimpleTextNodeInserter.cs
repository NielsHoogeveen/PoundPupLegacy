namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class SimpleTextNodeInserterFactory : DatabaseInserterFactory<SimpleTextNode>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableStringDatabaseParameter Text = new() { Name = "text" };
    internal static NonNullableStringDatabaseParameter Teaser = new() { Name = "teaser" };

    public override async Task<IDatabaseInserter<SimpleTextNode>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "simple_text_node",
            new DatabaseParameter[] {
                Id,
                Text,
                Teaser
            }
        );
        return new SimpleTextNodeInserter(command);
    }
}
internal sealed class SimpleTextNodeInserter : DatabaseInserter<SimpleTextNode>
{

    internal SimpleTextNodeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(SimpleTextNode simpleTextNode)
    {
        if (simpleTextNode.Id is null)
            throw new NullReferenceException();

        Set(SimpleTextNodeInserterFactory.Id, simpleTextNode.Id.Value);
        Set(SimpleTextNodeInserterFactory.Text, simpleTextNode.Text);
        Set(SimpleTextNodeInserterFactory.Teaser, simpleTextNode.Teaser);
        await _command.ExecuteNonQueryAsync();
    }
}
