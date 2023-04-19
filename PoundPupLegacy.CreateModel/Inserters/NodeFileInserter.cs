namespace PoundPupLegacy.CreateModel.Inserters;

using Request = NodeFile;

internal sealed class NodeFileInserterFactory : BasicDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    private static readonly NonNullableIntegerDatabaseParameter FileId = new() { Name = "file_id" };

    public override string TableName => "node_file";

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(NodeId, request.NodeId),
            ParameterValue.Create(FileId, request.FileId),
        };
    }
}
