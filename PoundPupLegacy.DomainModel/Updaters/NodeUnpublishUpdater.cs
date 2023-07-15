namespace PoundPupLegacy.DomainModel.Updaters;

using Request = NodeUnpublishRequest;

public sealed record NodeUnpublishRequest : IRequest
{
    public required int NodeId { get; init; }

}
internal sealed class NodeUnpublishFactory : DatabaseUpdaterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };

    public override string Sql => $"""
        update tenant_node 
        set 
            publication_status_id = 0
        where node_id = @node_id;
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(NodeId, request.NodeId),
        };
    }
}

