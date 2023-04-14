namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class TermHierarchyInserterFactory : DatabaseInserterFactory<TermHierarchy, TermHierarchyInserter>
{
    internal static NonNullableIntegerDatabaseParameter TermIdParent = new() { Name = "term_id_parent" };
    internal static NonNullableIntegerDatabaseParameter TermIdChild = new() { Name = "term_id_child" };

    public override string TableName => "term_hierarchy";
}
internal sealed class TermHierarchyInserter : DatabaseInserter<TermHierarchy>
{
    public TermHierarchyInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(TermHierarchy item)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TermHierarchyInserterFactory.TermIdParent, item.TermIdPartent),
            ParameterValue.Create(TermHierarchyInserterFactory.TermIdChild, item.TermIdChild),
        };
    }
}
