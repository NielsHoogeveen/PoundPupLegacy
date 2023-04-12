namespace PoundPupLegacy.CreateModel.Readers;
public sealed class NodeIdReaderByUrlIdFactory : DatabaseReaderFactory<NodeIdReaderByUrlId>
{
    internal static NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    internal static NonNullableIntegerDatabaseParameter UrlId = new() { Name = "url_id" };

    public override string Sql => SQL;

    const string SQL = """
        SELECT node_id FROM tenant_node WHERE tenant_id= @tenant_id AND url_id = @url_id
        """;
}
public sealed class NodeIdReaderByUrlId : SingleItemDatabaseReader<NodeIdReaderByUrlId.Request, int>
{
    public record Request
    {
        public int TenantId { get; init; }
        public int UrlId { get; init; }
    }


    internal NodeIdReaderByUrlId(NpgsqlCommand command) : base(command) { }

    public override async Task<int> ReadAsync(Request request)
    {

        _command.Parameters["tenant_id"].Value = request.TenantId;
        _command.Parameters["url_id"].Value = request.UrlId;

        var reader = await _command.ExecuteReaderAsync();
        if (reader.HasRows) {
            await reader.ReadAsync();
            var term = reader.GetInt32("node_id");
            await reader.CloseAsync();
            return term;
        }
        await reader.CloseAsync();
        throw new Exception($"node cannot be found in for url_id {request.UrlId} and tenant {request.TenantId}");
    }
}

