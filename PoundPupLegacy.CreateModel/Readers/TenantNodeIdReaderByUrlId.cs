namespace PoundPupLegacy.CreateModel.Readers;
public sealed class TenantNodeIdReaderByUrlIdFactory : IDatabaseReaderFactory<TenantNodeIdReaderByUrlId>
{
    public async Task<TenantNodeIdReaderByUrlId> CreateAsync(NpgsqlConnection connection)
    {
        var sql = """
        SELECT id FROM tenant_node WHERE tenant_id= @tenant_id AND url_id = @url_id
        """;

        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;

        command.Parameters.Add("tenant_id", NpgsqlDbType.Integer);
        command.Parameters.Add("url_id", NpgsqlDbType.Integer);
        await command.PrepareAsync();

        return new TenantNodeIdReaderByUrlId(command);

    }

}
public sealed class TenantNodeIdReaderByUrlId : SingleItemDatabaseReader<TenantNodeIdReaderByUrlId.Request, int>
{
    public record Request
    {
        public int TenantId { get; init; }
        public int UrlId { get; init; }
    }

    internal TenantNodeIdReaderByUrlId(NpgsqlCommand command) : base(command) { }

    public override async Task<int> ReadAsync(Request request)
    {

        _command.Parameters["tenant_id"].Value = request.TenantId;
        _command.Parameters["url_id"].Value = request.UrlId;

        var reader = await _command.ExecuteReaderAsync();
        if (reader.HasRows) {
            await reader.ReadAsync();
            var id = reader.GetInt32("id");
            await reader.CloseAsync();
            return id;
        }
        await reader.CloseAsync();
        throw new Exception($"tenant node cannot be found in for url_id {request.UrlId} and tenant {request.TenantId}");
    }
}

