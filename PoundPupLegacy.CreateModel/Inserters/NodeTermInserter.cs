namespace PoundPupLegacy.CreateModel.Inserters;
public sealed class NodeTermInserterFactory : DatabaseInserterFactory<NodeTerm, NodeTermInserter>
{
    internal static NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    internal static NonNullableIntegerDatabaseParameter TermId = new() { Name = "term_id" };

    public override string TableName => "node_term";

}
public sealed class NodeTermInserter : DatabaseInserter<NodeTerm>
{
    internal const string NODE_ID = "node_id";
    internal const string TERM_ID = "term_id";

    public NodeTermInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(NodeTerm item)
    {
        return new ParameterValue[] {
            ParameterValue.Create(NodeTermInserterFactory.NodeId, item.NodeId),
            ParameterValue.Create(NodeTermInserterFactory.TermId, item.TermId),
        };
    }

}
