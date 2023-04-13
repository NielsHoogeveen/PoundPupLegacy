namespace PoundPupLegacy.CreateModel.Readers;

using Request = EditOwnNodeActionIdReaderByNodeTypeIdRequest;
using Factory = EditOwnNodeActionIdReaderByNodeTypeIdFactory;
using Reader = EditOwnNodeActionIdReaderByNodeTypeId;


public sealed record EditOwnNodeActionIdReaderByNodeTypeIdRequest : IRequest
{
    public required int NodeTypeId { get; init; }
}

internal sealed class EditOwnNodeActionIdReaderByNodeTypeIdFactory : MandatorySingleItemDatabaseReaderFactory<Request, int, Reader>
{
    internal static NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };

    internal static IntValueReader IdReader = new() { Name = "id" };

    public override string Sql => SQL;
    const string SQL = """
        SELECT id FROM edit_own_node_action WHERE node_type_id = @node_type_id
        """;
}
internal sealed class EditOwnNodeActionIdReaderByNodeTypeId : IntDatabaseReader<Request>
{
    public EditOwnNodeActionIdReaderByNodeTypeId(NpgsqlCommand command) : base(command) { }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.NodeTypeId, request.NodeTypeId)
        };
    }

    protected override string GetErrorMessage(Request request)
    {
        return $"edit own node action cannot be found for node type {request.NodeTypeId}";
    }
    protected override IntValueReader IntValueReader => Factory.IdReader;
}
