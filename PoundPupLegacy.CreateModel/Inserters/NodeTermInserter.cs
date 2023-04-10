namespace PoundPupLegacy.CreateModel.Inserters;
public sealed class NodeTermInserterFactory : DatabaseInserterFactory<NodeTerm>
{
    internal static NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    internal static NonNullableIntegerDatabaseParameter TermId = new() { Name = "term_id" };

    public override async Task<IDatabaseInserter<NodeTerm>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "node_term",
            new DatabaseParameter[] {
                NodeId,
                TermId
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
        Set(NodeTermInserterFactory.NodeId, nodeTerm.NodeId);
        Set(NodeTermInserterFactory.TermId, nodeTerm.TermId);
        await _command.ExecuteNonQueryAsync();
    }
}
