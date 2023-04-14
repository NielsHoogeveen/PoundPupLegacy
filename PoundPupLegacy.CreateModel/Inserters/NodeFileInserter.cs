namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = NodeFileInserterFactory;
using Request = NodeFile;
using Inserter = NodeFileInserter;

internal sealed class NodeFileInserterFactory : DatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    internal static NonNullableIntegerDatabaseParameter FileId = new() { Name = "file_id" };

    public override string TableName => "node_file";
}
internal sealed class NodeFileInserter : DatabaseInserter<Request>
{
    public NodeFileInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(NodeFileInserterFactory.NodeId, request.NodeId),
            ParameterValue.Create(NodeFileInserterFactory.FileId, request.FileId),
        };
    }
}
