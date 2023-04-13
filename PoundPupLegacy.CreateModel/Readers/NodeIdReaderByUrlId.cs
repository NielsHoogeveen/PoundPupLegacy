namespace PoundPupLegacy.CreateModel.Readers;

using Request = NodeIdReaderByUrlIdRequest;
using Factory = NodeIdReaderByUrlIdFactory;
using Reader = NodeIdReaderByUrlId;

public sealed record NodeIdReaderByUrlIdRequest: IRequest
{
    public int TenantId { get; init; }
    public int UrlId { get; init; }
}

internal sealed class NodeIdReaderByUrlIdFactory : MandatorySingleItemDatabaseReaderFactory<Request, int, Reader>
{
    internal static NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    internal static NonNullableIntegerDatabaseParameter UrlId = new() { Name = "url_id" };

    internal static IntValueReader IdReader = new() { Name = "id" };

    public override string Sql => SQL;

    const string SQL = """
        SELECT node_id id FROM tenant_node WHERE tenant_id= @tenant_id AND url_id = @url_id
        """;
}
internal sealed class NodeIdReaderByUrlId : IntDatabaseReader<Request>
{

    public NodeIdReaderByUrlId(NpgsqlCommand command) : base(command) { }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.TenantId, request.TenantId),
            ParameterValue.Create(Factory.UrlId, request.UrlId)
        };
    }

    protected override IntValueReader IntValueReader => Factory.IdReader;

    protected override string GetErrorMessage(Request request)
    {
        return $"node cannot be found in for url_id {request.UrlId} and tenant {request.TenantId}";
    }
}

