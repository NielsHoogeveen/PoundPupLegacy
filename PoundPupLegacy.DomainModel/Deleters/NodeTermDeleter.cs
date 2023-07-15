using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Deleters;

using Request = NodeTermToRemove;

internal sealed class NodeTermDeleterFactory : DatabaseDeleterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    private static readonly NonNullableIntegerDatabaseParameter TermId = new() { Name = "term_id" };

    public override string Sql => SQL;

    const string SQL = $"""
        delete from node_term
        where node_id = @node_id and term_id = @term_id;
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(NodeId, request.NodeId),
            ParameterValue.Create(TermId, request.TermId),
        };
    }
}
