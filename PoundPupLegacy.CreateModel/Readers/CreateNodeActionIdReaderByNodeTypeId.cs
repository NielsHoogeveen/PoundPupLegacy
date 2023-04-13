namespace PoundPupLegacy.CreateModel.Readers;

using Request = CreateNodeActionIdReaderByNodeTypeIdRequest;
using Factory = CreateNodeActionIdReaderByNodeTypeIdFactory;
using Reader = CreateNodeActionIdReaderByNodeTypeId;

public sealed class CreateNodeActionIdReaderByNodeTypeIdRequest : IRequest
{
    public required int NodeTypeId { get; init; }
}

internal sealed class CreateNodeActionIdReaderByNodeTypeIdFactory : MandatorySingleItemDatabaseReaderFactory<Request, int, Reader>
{
    internal static NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };

    internal static IntValueReader IdReader = new() { Name = "id" };
    public override string Sql => SQL;

    private const string SQL = @"
        SELECT id FROM create_node_action WHERE node_type_id = @node_type_id
        ";
}
internal sealed class CreateNodeActionIdReaderByNodeTypeId : IntDatabaseReader<Request>
{
    public CreateNodeActionIdReaderByNodeTypeId(NpgsqlCommand command) : base(command) { }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.NodeTypeId, request.NodeTypeId)
        };
    }

    protected override string GetErrorMessage(Request request)
    {
        return $"create node action cannot be found for node type  {request}";
    }
    protected override IntValueReader IntValueReader => Factory.IdReader;

}
