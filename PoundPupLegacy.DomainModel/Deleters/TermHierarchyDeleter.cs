namespace PoundPupLegacy.DomainModel.Deleters;

using Request = TermHierarchyToRemoveRequest;

public record TermHierarchyToRemoveRequest: IRequest
{
    public required int TermIdParent { get; init; }
    public required int TermIdChild { get; init; }
}

internal sealed class TermHierarchyDeleterFactory : DatabaseDeleterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter TermIdParent = new() { Name = "term_id_parent" };
    private static readonly NonNullableIntegerDatabaseParameter TermIdChild = new() { Name = "term_id_child" };

    public override string Sql => SQL;

    const string SQL = $"""
        delete from term_hierarchy
        where term_id_parent = @term_id_parent and term_id_child = @term_id_child;
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TermIdParent, request.TermIdParent),
            ParameterValue.Create(TermIdChild, request.TermIdChild),
        };
    }
}
