namespace PoundPupLegacy.CreateModel.Readers;

using Request = TenantNodeReaderByUrlIdRequest;
using Factory = TenantNodeReaderByUrlIdFactory;
using Reader = TenantNodeReaderByUrlId;

public sealed class TenantNodeReaderByUrlIdRequest : IRequest
{
    public required int TenantId { get; init; }
    public required int UrlId { get; init; }
}
internal sealed class TenantNodeReaderByUrlIdFactory : SingleItemDatabaseReaderFactory<Request, TenantNode, Reader>
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

}
internal sealed class TenantNodeReaderByUrlId : SingleItemDatabaseReader<Request, TenantNode>
{

    public TenantNodeReaderByUrlId(NpgsqlCommand command) : base(command) { }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.TenantId, request.TenantId),
            ParameterValue.Create(Factory.UrlId, request.UrlId),
        };
    }

    protected override TenantNode Read(NpgsqlDataReader reader)
    {
        return new TenantNode {
            Id = Factory.IdReader.GetValue(reader),
            TenantId = Factory.TenantIdReader.GetValue(reader),
            UrlId = Factory.UrlIdReader.GetValue(reader),
            NodeId = Factory.NodeIdReader.GetValue(reader),
            UrlPath = Factory.UrlPathReader.GetValue(reader),
            PublicationStatusId = Factory.PublicationStatusIdReader.GetValue(reader),
            SubgroupId = Factory.SubgroupIdReader.GetValue(reader),
        };
    }
}

