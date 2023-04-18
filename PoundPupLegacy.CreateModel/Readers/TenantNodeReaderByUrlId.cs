namespace PoundPupLegacy.CreateModel.Readers;

using Request = TenantNodeReaderByUrlIdRequest;

public sealed class TenantNodeReaderByUrlIdRequest : IRequest
{
    public required int TenantId { get; init; }
    public required int UrlId { get; init; }
}
internal sealed class TenantNodeReaderByUrlIdFactory : SingleItemDatabaseReaderFactory<Request, TenantNode>
{
    internal static NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    internal static NonNullableIntegerDatabaseParameter UrlId = new() { Name = "url_id" };

    internal static IntValueReader IdReader = new() { Name = "id" };
    internal static IntValueReader NodeIdReader = new() { Name = "node_id" };
    internal static IntValueReader TenantIdReader = new() { Name = "tenant_id" };
    internal static IntValueReader UrlIdReader = new() { Name = "url_id" };
    internal static IntValueReader PublicationStatusIdReader = new() { Name = "publication_status_id" };
    internal static NullableIntValueReader SubgroupIdReader = new() { Name = "subgroup_id" };
    internal static NullableStringValueReader UrlPathReader = new() { Name = "url_path" };

    public override string Sql => SQL;

    const string SQL = """
        SELECT 
            id,
            node_id,
            tenant_id,
            url_id,
            url_path,
            publication_status_id,
            subgroup_id
        FROM tenant_node 
        WHERE tenant_id= @tenant_id AND url_id = @url_id
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantId, request.TenantId),
            ParameterValue.Create(UrlId, request.UrlId),
        };
    }

    protected override TenantNode Read(NpgsqlDataReader reader)
    {
        return new TenantNode {
            Id = IdReader.GetValue(reader),
            TenantId = TenantIdReader.GetValue(reader),
            UrlId = UrlIdReader.GetValue(reader),
            NodeId = NodeIdReader.GetValue(reader),
            UrlPath = UrlPathReader.GetValue(reader),
            PublicationStatusId = PublicationStatusIdReader.GetValue(reader),
            SubgroupId = SubgroupIdReader.GetValue(reader),
        };
    }
}

