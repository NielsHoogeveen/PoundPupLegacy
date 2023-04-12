namespace PoundPupLegacy.CreateModel.Readers;

using Factory = NodeIdReaderByUrlIdFactory;
using Reader = NodeIdReaderByUrlId;

public sealed class NodeIdReaderByUrlIdFactory : DatabaseReaderFactory<Reader>
{
    internal static NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    internal static NonNullableIntegerDatabaseParameter UrlId = new() { Name = "url_id" };

    internal static IntValueReader IdReader = new() { Name = "id" };

    public override string Sql => SQL;

    const string SQL = """
        SELECT node_id id FROM tenant_node WHERE tenant_id= @tenant_id AND url_id = @url_id
        """;
}
public sealed class NodeIdReaderByUrlId : IntDatabaseReader<Reader.Request>
{
    public record Request
    {
        public int TenantId { get; init; }
        public int UrlId { get; init; }
    }


    internal NodeIdReaderByUrlId(NpgsqlCommand command) : base(command) { }

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

