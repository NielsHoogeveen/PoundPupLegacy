namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class NodeFileInserterFactory : DatabaseInserterFactory<NodeFile, NodeFileInserter>
{
    internal static NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    internal static NonNullableIntegerDatabaseParameter FileId = new() { Name = "file_id" };

    public override string TableName => "node_file";
}
internal sealed class NodeFileInserter : DatabaseInserter<NodeFile>
{
    public NodeFileInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(NodeFile item)
    {
        return new ParameterValue[] {
            ParameterValue.Create(NodeFileInserterFactory.NodeId, item.NodeId),
            ParameterValue.Create(NodeFileInserterFactory.FileId, item.FileId),
        };
    }
}
