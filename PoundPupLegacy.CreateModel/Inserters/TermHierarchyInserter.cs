namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class TermHierarchyInserterFactory : DatabaseInserterFactory<TermHierarchy>
{
    public override async Task<IDatabaseInserter<TermHierarchy>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "term_hierarchy",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = TermHierarchyInserter.TERM_ID_PARENT,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = TermHierarchyInserter.TERM_ID_CHILD,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new TermHierarchyInserter(command);
    }
}
internal sealed class TermHierarchyInserter : DatabaseInserter<TermHierarchy>
{
    internal const string TERM_ID_PARENT = "term_id_parent";
    internal const string TERM_ID_CHILD = "term_id_child";
    internal TermHierarchyInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(TermHierarchy termHierarchy)
    {
        WriteValue(termHierarchy.TermIdPartent, TERM_ID_PARENT);
        WriteValue(termHierarchy.TermIdChild, TERM_ID_CHILD);
        await _command.ExecuteNonQueryAsync();
    }
}
