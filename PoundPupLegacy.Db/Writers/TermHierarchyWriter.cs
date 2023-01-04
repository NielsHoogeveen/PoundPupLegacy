namespace PoundPupLegacy.Db.Writers;

internal class TermHierarchyWriter : DatabaseWriter<TermHierarchy>, IDatabaseWriter<TermHierarchy>
{
    private const string TERM_ID_PARENT = "term_id_parent";
    private const string TERM_ID_CHILD = "term_id_child";
    public static async Task<DatabaseWriter<TermHierarchy>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "term_hierarchy",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = TERM_ID_PARENT,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = TERM_ID_CHILD,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );

        return new TermHierarchyWriter(command);

    }
    private TermHierarchyWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(TermHierarchy termHierarchy)
    {
        WriteValue(termHierarchy.TermIdPartent, TERM_ID_PARENT);
        WriteValue(termHierarchy.TermIdChild, TERM_ID_CHILD);
        await _command.ExecuteNonQueryAsync();
    }
}
