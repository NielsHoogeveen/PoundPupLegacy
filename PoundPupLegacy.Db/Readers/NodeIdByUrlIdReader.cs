using System.Data;

namespace PoundPupLegacy.Db.Readers;

public sealed class NodeIdByUrlIdReader : DatabaseReader<Term>, IDatabaseReader<NodeIdByUrlIdReader>
{
    public static async Task<NodeIdByUrlIdReader> CreateAsync(NpgsqlConnection connection)
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

        return new NodeIdByUrlIdReader(command);

    }

    internal NodeIdByUrlIdReader(NpgsqlCommand command) : base(command) { }

    public async Task<int> ReadAsync(int tenantId, int urlId)
    {
        
        _command.Parameters["tenant_id"].Value = tenantId;
        _command.Parameters["url_id"].Value = urlId;

        var reader = await _command.ExecuteReaderAsync();
        if (reader.HasRows)
        {
            await reader.ReadAsync();
            var term = reader.GetInt32("node_id");
            await reader.CloseAsync();
            return term;
        }
        await reader.CloseAsync();
        throw new Exception($"node cannot be found in for url_id {urlId} and tenant {tenantId}");
    }
}

