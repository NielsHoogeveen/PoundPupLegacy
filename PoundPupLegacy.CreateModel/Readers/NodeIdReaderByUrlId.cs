namespace PoundPupLegacy.CreateModel.Readers;

using Request = NodeIdReaderByUrlIdRequest;

public sealed record NodeIdReaderByUrlIdRequest: IRequest
{
    public int TenantId { get; init; }
    public int UrlId { get; init; }
}

internal sealed class NodeIdReaderByUrlIdFactory : IntDatabaseReaderFactory<Request>
{
    internal static NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    internal static NonNullableIntegerDatabaseParameter UrlId = new() { Name = "url_id" };

    internal static IntValueReader IdReader = new() { Name = "id" };

    public override string Sql => SQL;

    const string SQL = """
        SELECT node_id id FROM tenant_node WHERE tenant_id= @tenant_id AND url_id = @url_id
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantId, request.TenantId),
            ParameterValue.Create(UrlId, request.UrlId)
        };
    }

    protected override IntValueReader IntValueReader => IdReader;

    protected override string GetErrorMessage(Request request)
    {
        return $"node cannot be found in for url_id {request.UrlId} and tenant {request.TenantId}";
    }
}
