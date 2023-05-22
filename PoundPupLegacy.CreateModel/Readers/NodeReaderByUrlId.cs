namespace PoundPupLegacy.CreateModel.Readers;

using Request = NodeReaderByUrlIdRequest;

public sealed record NodeReaderByUrlIdRequest : IRequest
{
    public required int TenantId { get; init; }
    public required int UrlId { get; init; }
}

internal sealed class NodeReaderByUrlIdFactory : MandatorySingleItemDatabaseReaderFactory<Request, EventuallyIdentifiableNode>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UrlId = new() { Name = "url_id" };

    private static readonly IntValueReader IdReader = new() { Name = "id" };
    private static readonly IntValueReader PublisherIdReader = new() { Name = "publisher_id" };
    private static readonly StringValueReader TitleReader = new() { Name = "title" };
    private static readonly DateTimeValueReader CreatedDateTimeReader = new() { Name = "created_date_time" };
    private static readonly DateTimeValueReader ChangedDateTimeReader = new() { Name = "changed_date_time" };
    private static readonly IntValueReader NodeTypeIdReader = new() { Name = "node_type_id" };
    private static readonly IntValueReader OwnerIdReader = new() { Name = "owner_id" };

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
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantId, request.TenantId),
            ParameterValue.Create(UrlId, request.UrlId),
        };
    }

    protected override EventuallyIdentifiableNode Read(NpgsqlDataReader reader)
    {
        var node = new NewBasicNode {
            Id = IdReader.GetValue(reader),
            PublisherId = PublisherIdReader.GetValue(reader),
            Title = TitleReader.GetValue(reader),
            CreatedDateTime = CreatedDateTimeReader.GetValue(reader),
            ChangedDateTime = ChangedDateTimeReader.GetValue(reader),
            NodeTypeId = NodeTypeIdReader.GetValue(reader),
            OwnerId = OwnerIdReader.GetValue(reader),
            AuthoringStatusId = 1,
            TenantNodes = new List<TenantNode>(),
        };
        return node;
    }

    protected override string GetErrorMessage(Request request)
    {
        return $"node cannot be found in for url_id {request.UrlId} and tenant {request.TenantId}";
    }
}
