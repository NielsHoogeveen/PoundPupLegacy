namespace PoundPupLegacy.CreateModel.Readers;

using Request = EditNodeActionIdReaderByNodeTypeIdRequest;
using Factory = EditNodeActionIdReaderByNodeTypeIdFactory;
using Reader = EditNodeActionIdReaderByNodeTypeId;

public sealed record EditNodeActionIdReaderByNodeTypeIdRequest: IRequest
{
    public required int NodeTypeId { get; init; }
}

internal sealed class EditNodeActionIdReaderByNodeTypeIdFactory : MandatorySingleItemDatabaseReaderFactory<Request, int, Reader>
{
    internal static NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };

    internal static IntValueReader IdReader = new() { Name = "id" };

    public override string Sql => SQL;

    const string SQL = """
        SELECT id FROM edit_node_action WHERE node_type_id = @node_type_id
        """;
}

internal sealed class EditNodeActionIdReaderByNodeTypeId : IntDatabaseReader<Request>
{
    public EditNodeActionIdReaderByNodeTypeId(NpgsqlCommand command) : base(command) { }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.NodeTypeId, request.NodeTypeId)
        };
    }
    protected override IntValueReader IntValueReader => Factory.IdReader;

    protected override string GetErrorMessage(Request request)
    {
        return $"edit node action cannot be found for node type { request.NodeTypeId }";
    }
}
