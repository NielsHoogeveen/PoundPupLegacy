namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = NodeTermInserterFactory;
using Request = NodeTerm;
using Inserter = NodeTermInserter;

public sealed class NodeTermInserterFactory : DatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    internal static NonNullableIntegerDatabaseParameter TermId = new() { Name = "term_id" };

    public override string TableName => "node_term";

}
public sealed class NodeTermInserter : DatabaseInserter<Request>
{
    internal const string NODE_ID = "node_id";
    internal const string TERM_ID = "term_id";

    public NodeTermInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.NodeId, request.NodeId),
            ParameterValue.Create(Factory.TermId, request.TermId),
        };
    }
}
