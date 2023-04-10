namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class TermHierarchyInserterFactory : DatabaseInserterFactory<TermHierarchy>
{
    internal static NonNullableIntegerDatabaseParameter TermIdParent = new() { Name = "term_id_parent" };
    internal static NonNullableIntegerDatabaseParameter TermIdChild = new() { Name = "term_id_child" };

    public override async Task<IDatabaseInserter<TermHierarchy>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "term_hierarchy",
            new DatabaseParameter[] {
                TermIdParent,
                TermIdChild
            }
        );
        return new TermHierarchyInserter(command);
    }
}
internal sealed class TermHierarchyInserter : DatabaseInserter<TermHierarchy>
{
    internal TermHierarchyInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(TermHierarchy termHierarchy)
    {
        Set(TermHierarchyInserterFactory.TermIdParent, termHierarchy.TermIdPartent);
        Set(TermHierarchyInserterFactory.TermIdChild, termHierarchy.TermIdChild);
        await _command.ExecuteNonQueryAsync();
    }
}
