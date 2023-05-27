namespace PoundPupLegacy.CreateModel.Inserters;

using Request = NodeTermToAdd;

public sealed class NodeTermInserterFactory : BasicDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    private static readonly NonNullableIntegerDatabaseParameter TermId = new() { Name = "term_id" };

    public override string TableName => "node_term";

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(NodeId, request.NodeId),
            ParameterValue.Create(TermId, request.TermId),
        };
    }
}
