using System.Data;

namespace PoundPupLegacy.Db.Readers;

public sealed class TenantNodeIdReaderByUrlId : DatabaseUpdater<Term>, IDatabaseReader<TenantNodeIdReaderByUrlId>
{
    public static async Task<TenantNodeIdReaderByUrlId> CreateAsync(NpgsqlConnection connection)
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

    internal TenantNodeIdReaderByUrlId(NpgsqlCommand command) : base(command) { }

    public async Task<int> ReadAsync(int tenantId, int urlId)
    {

        _command.Parameters["tenant_id"].Value = tenantId;
        _command.Parameters["url_id"].Value = urlId;

        var reader = await _command.ExecuteReaderAsync();
        if (reader.HasRows)
        {
            await reader.ReadAsync();
            var id = reader.GetInt32("id");
            await reader.CloseAsync();
            return id;
        }
        await reader.CloseAsync();
        throw new Exception($"tenant node cannot be found in for url_id {urlId} and tenant {tenantId}");
    }
}

