namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class NodeTypeInserterFactory : DatabaseInserterFactory<NodeType>
{
    public override async Task<IDatabaseInserter<NodeType>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "node_type",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = NodeTypeInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = NodeTypeInserter.NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = NodeTypeInserter.DESCRIPTION,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = NodeTypeInserter.AUTHOR_SPECIFIC,
                    NpgsqlDbType = NpgsqlDbType.Boolean
                },
            }
        );
        return new NodeTypeInserter(command);
    }
}
internal sealed class NodeTypeInserter : DatabaseInserter<NodeType>
{

    internal const string ID = "id";
    internal const string NAME = "name";
    internal const string DESCRIPTION = "description";
    internal const string AUTHOR_SPECIFIC = "author_specific";

    internal NodeTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(NodeType nodeType)
    {
        SetParameter(nodeType.Id, ID);
        SetNullableParameter(nodeType.Name, NAME);
        SetNullableParameter(nodeType.Description, DESCRIPTION);
        SetParameter(nodeType.AuthorSpecific, AUTHOR_SPECIFIC);
        await _command.ExecuteNonQueryAsync();
    }
}
