using System.Data;

namespace PoundPupLegacy.Db.Readers;

public sealed class TenantNodeReaderByUrlId : DatabaseUpdater<Term>, IDatabaseReader<TenantNodeReaderByUrlId>
{
    public static async Task<TenantNodeReaderByUrlId> CreateAsync(NpgsqlConnection connection)
    {
        var sql = """
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

        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;

        command.Parameters.Add("tenant_id", NpgsqlDbType.Integer);
        command.Parameters.Add("url_id", NpgsqlDbType.Integer);
        await command.PrepareAsync();

        return new TenantNodeReaderByUrlId(command);

    }

    internal TenantNodeReaderByUrlId(NpgsqlCommand command) : base(command) { }

    public async Task<TenantNode?> ReadAsync(int tenantId, int urlId)
    {

        _command.Parameters["tenant_id"].Value = tenantId;
        _command.Parameters["url_id"].Value = urlId;

        var reader = await _command.ExecuteReaderAsync();
        if (reader.HasRows)
        {
            await reader.ReadAsync();
            var tenantNode = new TenantNode
            {
                Id = reader.GetInt32("id"),
                TenantId = reader.GetInt32("tenant_id"),
                UrlId = reader.GetInt32("url_id"),
                NodeId = reader.GetInt32("node_id"),
                UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                PublicationStatusId = reader.GetInt32("publication_status_id"),
                SubgroupId = reader.IsDBNull("subgroup_id") ? null : reader.GetInt32("subgroup_id"),
            };
            await reader.CloseAsync();
            return tenantNode;
        }
        else
        {
            await reader.CloseAsync();
            return null;
        }
    }
}

