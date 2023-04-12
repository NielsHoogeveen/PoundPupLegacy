namespace PoundPupLegacy.CreateModel.Readers;

using Factory = NodeReaderByUrlIdFactory;
using Reader = NodeReaderByUrlId;
public sealed class NodeReaderByUrlIdFactory : DatabaseReaderFactory<Reader>
{
    internal static NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    internal static NonNullableIntegerDatabaseParameter UrlId = new() { Name = "url_id" };

    internal static IntValueReader IdReader = new() { Name = "id" };
    internal static IntValueReader PublisherIdReader = new() { Name = "publisher_id" };
    internal static StringValueReader TitleReader = new() { Name = "title" };
    internal static DateTimeValueReader CreatedDateTimeReader = new() { Name = "created_date_time" };
    internal static DateTimeValueReader ChangedDateTimeReader = new() { Name = "changed_date_time" };
    internal static IntValueReader NodeTypeIdReader = new() { Name = "node_type_id" };
    internal static IntValueReader OwnerIdReader = new() { Name = "owner_id" };

    public override string Sql => SQL;

    const string SQL = """
        SELECT 
        n.id,
        n.publisher_id,
        n.title,
        n.created_date_time,
        n.changed_date_time,
        n.node_type_id,
        n.owner_id
        FROM tenant_node t
        join node n on n.id = t.node_id
        WHERE t.tenant_id= @tenant_id AND t.url_id = @url_id
        """;
}
public sealed class NodeReaderByUrlId : MandatorySingleItemDatabaseReader<Reader.Request, Node>
{
    public record Request
    {
        public required int TenantId { get; init; }
        public required int UrlId { get; init; }

    }

    internal NodeReaderByUrlId(NpgsqlCommand command) : base(command) { }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.TenantId, request.TenantId),
            ParameterValue.Create(Factory.UrlId, request.UrlId),
        };
    }

    protected override Node Read(NpgsqlDataReader reader)
    {
        var node = new BasicNode {
            Id = Factory.IdReader.GetValue(reader),
            PublisherId = Factory.PublisherIdReader.GetValue(reader),
            Title = Factory.TitleReader.GetValue(reader),
            CreatedDateTime = Factory.CreatedDateTimeReader.GetValue(reader),
            ChangedDateTime = Factory.ChangedDateTimeReader.GetValue(reader),
            NodeTypeId = Factory.NodeTypeIdReader.GetValue(reader),
            OwnerId = Factory.OwnerIdReader.GetValue(reader),
            TenantNodes = new List<TenantNode>(),
        };
        return node;
    }

    protected override string GetErrorMessage(Request request)
    {
        return $"node cannot be found in for url_id {request.UrlId} and tenant {request.TenantId}";
    }
}

