namespace PoundPupLegacy.CreateModel.Inserters;
public sealed class NodeTermInserterFactory : DatabaseInserterFactory<NodeTerm>
{
    public override async Task<IDatabaseInserter<NodeTerm>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "node_term",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = NodeTermInserter.NODE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = NodeTermInserter.TERM_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new NodeTermInserter(command);
    }
}
public sealed class NodeTermInserter : DatabaseInserter<NodeTerm>
{
    internal const string NODE_ID = "node_id";
    internal const string TERM_ID = "term_id";

    internal NodeTermInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(NodeTerm nodeTerm)
    {
        SetParameter(nodeTerm.NodeId, NODE_ID);
        SetParameter(nodeTerm.TermId, TERM_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
