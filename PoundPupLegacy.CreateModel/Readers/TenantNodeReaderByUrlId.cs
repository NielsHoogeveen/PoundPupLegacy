namespace PoundPupLegacy.DomainModel.Readers;

using Request = TenantNodeReaderByUrlIdRequest;

public sealed class TenantNodeReaderByUrlIdRequest : IRequest
{
    public required int TenantId { get; init; }
    public required int UrlId { get; init; }
}
internal sealed class TenantNodeReaderByUrlIdFactory : SingleItemDatabaseReaderFactory<Request, TenantNode.ToCreate.ForExistingNode>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UrlId = new() { Name = "url_id" };

    private static readonly IntValueReader IdReader = new() { Name = "id" };
    private static readonly IntValueReader NodeIdReader = new() { Name = "node_id" };
    private static readonly IntValueReader TenantIdReader = new() { Name = "tenant_id" };
    private static readonly IntValueReader UrlIdReader = new() { Name = "url_id" };
    private static readonly IntValueReader PublicationStatusIdReader = new() { Name = "publication_status_id" };
    private static readonly NullableIntValueReader SubgroupIdReader = new() { Name = "subgroup_id" };
    private static readonly NullableStringValueReader UrlPathReader = new() { Name = "url_path" };

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

    protected override TenantNode.ToCreate.ForExistingNode Read(NpgsqlDataReader reader)
    {
        return new TenantNode.ToCreate.ForExistingNode {
            Identification = new Identification.Possible {
                Id = IdReader.GetValue(reader),
            },
            TenantId = TenantIdReader.GetValue(reader),
            UrlId = UrlIdReader.GetValue(reader),
            NodeId = NodeIdReader.GetValue(reader),
            UrlPath = UrlPathReader.GetValue(reader),
            PublicationStatusId = PublicationStatusIdReader.GetValue(reader),
            SubgroupId = SubgroupIdReader.GetValue(reader),
        };
    }
}

