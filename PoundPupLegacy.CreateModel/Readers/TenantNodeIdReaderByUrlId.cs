namespace PoundPupLegacy.CreateModel.Readers;

using Request = TenantNodeIdReaderByUrlIdRequest;
using Factory = TenantNodeIdReaderByUrlIdFactory;
using Reader = TenantNodeIdReaderByUrlId;

public sealed class TenantNodeIdReaderByUrlIdRequest : IRequest
{
    public required int TenantId { get; init; }
    public required int UrlId { get; init; }
}

internal sealed class TenantNodeIdReaderByUrlIdFactory : MandatorySingleItemDatabaseReaderFactory<Request, int, Reader>
{
    internal static NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    internal static NonNullableIntegerDatabaseParameter UrlId = new() { Name = "url_id" };

    internal static IntValueReader IdReader = new() { Name = "id" };

    public override string Sql => SQL;

    const string SQL = """
        SELECT id FROM tenant_node WHERE tenant_id= @tenant_id AND url_id = @url_id
        """;
}
internal sealed class TenantNodeIdReaderByUrlId : IntDatabaseReader<Request>
{

    public TenantNodeIdReaderByUrlId(NpgsqlCommand command) : base(command) { }

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
        return $"tenant node cannot be found in for url_id {request.UrlId} and tenant {request.TenantId}";
    }

}

