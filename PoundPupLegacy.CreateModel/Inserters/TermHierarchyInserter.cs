namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class TermHierarchyInserterFactory : BasicDatabaseInserterFactory<TermHierarchy, TermHierarchyInserter>
{
    internal static NonNullableIntegerDatabaseParameter TermIdParent = new() { Name = "term_id_parent" };
    internal static NonNullableIntegerDatabaseParameter TermIdChild = new() { Name = "term_id_child" };

    public override string TableName => "term_hierarchy";
}
internal sealed class TermHierarchyInserter : BasicDatabaseInserter<TermHierarchy>
{
    public TermHierarchyInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(TermHierarchy item)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TermHierarchyInserterFactory.TermIdParent, item.TermIdPartent),
            ParameterValue.Create(TermHierarchyInserterFactory.TermIdChild, item.TermIdChild),
        };
    }
}
