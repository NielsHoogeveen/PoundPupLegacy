namespace PoundPupLegacy.CreateModel.Inserters;

using Request = NodeTerm;

public sealed class NodeTermInserterFactory : BasicDatabaseInserterFactory<Request>
{
    internal static NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    internal static NonNullableIntegerDatabaseParameter TermId = new() { Name = "term_id" };

    public override string TableName => "node_term";

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(NodeId, request.NodeId),
            ParameterValue.Create(TermId, request.TermId),
        };
    }
}
