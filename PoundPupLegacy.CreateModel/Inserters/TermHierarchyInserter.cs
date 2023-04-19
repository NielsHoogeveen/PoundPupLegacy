namespace PoundPupLegacy.CreateModel.Inserters;

using Request = TermHierarchy;

internal sealed class TermHierarchyInserterFactory : BasicDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter TermIdParent = new() { Name = "term_id_parent" };
    private static readonly NonNullableIntegerDatabaseParameter TermIdChild = new() { Name = "term_id_child" };

    public override string TableName => "term_hierarchy";
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TermIdParent, request.TermIdPartent),
            ParameterValue.Create(TermIdChild, request.TermIdChild),
        };
    }
}
