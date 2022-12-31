namespace PoundPupLegacy.Db.Writers;

internal class TermHierarchyWriter : DatabaseWriter<TermHierarchy>, IDatabaseWriter<TermHierarchy>
{
    private const string TERM_ID_PARENT = "term_id_parent";
    private const string TERM_ID_CHILD = "term_id_child";
    public static DatabaseWriter<TermHierarchy> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
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

    internal override void Write(TermHierarchy termHierarchy)
    {
        WriteValue(termHierarchy.ParentId, TERM_ID_PARENT);
        WriteValue(termHierarchy.ChildId, TERM_ID_CHILD);
        _command.ExecuteNonQuery();
    }
}
