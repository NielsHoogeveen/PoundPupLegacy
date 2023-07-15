namespace PoundPupLegacy.DomainModel.Readers;

using Request = EditOwnNodeActionIdReaderByNodeTypeIdRequest;

public sealed record EditOwnNodeActionIdReaderByNodeTypeIdRequest : IRequest
{
    public required int NodeTypeId { get; init; }
}

internal sealed class EditOwnNodeActionIdReaderByNodeTypeIdFactory : IntDatabaseReaderFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };

    private static readonly IntValueReader IdReader = new() { Name = "id" };

    public override string Sql => SQL;
    const string SQL = """
        SELECT id FROM edit_own_node_action WHERE node_type_id = @node_type_id
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(NodeTypeId, request.NodeTypeId)
        };
    }

    protected override string GetErrorMessage(Request request)
    {
        return $"edit own node action cannot be found for node type {request.NodeTypeId}";
    }
    protected override IntValueReader IntValueReader => IdReader;
}
