namespace PoundPupLegacy.ViewModel.Readers;

using Request = NodeTypeReaderRequest;

public sealed record NodeTypeReaderRequest : IRequest
{
    public required int TenantId { get; init; }
    public required int UserId { get; init; }
    public required int UrlId { get; init; }
}
public sealed record NodeTypeResponse
{
    public required int NodeTypeId { get; init; }
}

internal sealed class NodeTypeReaderFactory : SingleItemDatabaseReaderFactory<Request, NodeTypeResponse>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NonNullableIntegerDatabaseParameter UrlIdParameter = new() { Name = "url_id" };

    public override string Sql => SQL;

    const string SQL = $"""
        WITH 
        {SharedSql.ACCESSIBLE_PUBLICATIONS_STATUS}
        SELECT 
        n.node_type_id
        from tenant_node tn
        join node n on n.id = tn.node_id
        where tn.tenant_id = @tenant_id and tn.url_id = @url_id
        and tn.publication_status_id in (
            select 
            id 
            from accessible_publication_status 
            where tenant_id = tn.tenant_id 
            and (
                subgroup_id = tn.subgroup_id 
                or subgroup_id is null and tn.subgroup_id is null
            )
        )
        """;


    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(UrlIdParameter, request.UrlId),
            ParameterValue.Create(TenantIdParameter, request.TenantId),
            ParameterValue.Create(UserIdParameter, request.UserId),
        };
    }

    protected override NodeTypeResponse? Read(NpgsqlDataReader reader)
    {
        var node_type_id = reader.GetInt32(0);
        return new NodeTypeResponse { NodeTypeId = node_type_id };

    }
}
