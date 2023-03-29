namespace PoundPupLegacy.CreateModel.Readers;
public sealed class NodeIdReaderByUrlIdFactory : IDatabaseReaderFactory<NodeIdReaderByUrlId>
{
    public async Task<NodeIdReaderByUrlId> CreateAsync(NpgsqlConnection connection)
    {
        var sql = """
        SELECT node_id FROM tenant_node WHERE tenant_id= @tenant_id AND url_id = @url_id
        """;

        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;

        command.Parameters.Add("tenant_id", NpgsqlDbType.Integer);
        command.Parameters.Add("url_id", NpgsqlDbType.Integer);
        await command.PrepareAsync();

        return new NodeIdReaderByUrlId(command);

    }

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

