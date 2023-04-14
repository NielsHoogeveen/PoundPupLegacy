namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = TermHierarchyInserterFactory;
using Request = TermHierarchy;
using Inserter = TermHierarchyInserter;

internal sealed class TermHierarchyInserterFactory : DatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter TermIdParent = new() { Name = "term_id_parent" };
    internal static NonNullableIntegerDatabaseParameter TermIdChild = new() { Name = "term_id_child" };

    public override string TableName => "term_hierarchy";
}
internal sealed class TermHierarchyInserter : DatabaseInserter<Request>
{
    public TermHierarchyInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.TermIdParent, request.TermIdPartent),
            ParameterValue.Create(Factory.TermIdChild, request.TermIdChild),
        };
    }
}
