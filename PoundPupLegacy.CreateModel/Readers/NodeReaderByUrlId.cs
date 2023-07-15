namespace PoundPupLegacy.DomainModel.Readers;

using Request = NodeReaderByUrlIdRequest;

public sealed record NodeReaderByUrlIdRequest : IRequest
{
    public required int TenantId { get; init; }
    public required int UrlId { get; init; }
}

public sealed record NodeTitle
{
    public required int Id { get; init; }
    public required string Title { get; init; }
}

internal sealed class NodeReaderByUrlIdFactory : MandatorySingleItemDatabaseReaderFactory<Request, NodeTitle>
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
        n.title,
        n.changed_date_time
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

    protected override NodeTitle Read(NpgsqlDataReader reader)
    {
        var node = new NodeTitle {
            Id = IdReader.GetValue(reader),
            Title = TitleReader.GetValue(reader),
        };
        return node;
    }

    protected override string GetErrorMessage(Request request)
    {
        return $"node cannot be found in for url_id {request.UrlId} and tenant {request.TenantId}";
    }
}
