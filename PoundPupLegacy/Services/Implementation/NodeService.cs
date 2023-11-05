using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.Services.Implementation;

public class NodeService(
    NpgsqlDataSource dataSource, ILogger<NodeService> logger
) : DatabaseService(dataSource, logger), INodeService
{
    public async Task<string?> GetRedirectPath(int urlId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            var command = connection.CreateCommand();
            command.CommandText = """
                SELECT
                '/' || nt.viewer_path || '/' || tn.node_id as path
                FROM tenant_node tn
                join node n on n.id = tn.node_id
                join node_type nt on nt.id = n.node_type_id
                WHERE tn.url_id = @url_id 
                AND tn.tenant_id = @tenant_id
                """;
            command.Parameters.Add("url_id", NpgsqlTypes.NpgsqlDbType.Integer);
            command.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
            await command.PrepareAsync();
            command.Parameters["url_id"].Value = urlId;
            command.Parameters["tenant_id"].Value = tenantId;

            var reader = await command.ExecuteReaderAsync();
            if(await reader.ReadAsync()) {
                return reader.GetString(0);
            }
            return null;
        });
    }
    public async Task<string?> GetRedirectPath(string urlPath, int tenantId)
    {
        return await WithConnection(async (connection) => {
            var command = connection.CreateCommand();
            command.CommandText = """
                SELECT
                '/' || nt.viewer_path || '/' || tn.node_id as path
                FROM tenant_node tn
                join node n on n.id = tn.node_id
                join node_type nt on nt.id = n.node_type_id
                WHERE tn.url_path = @url_path 
                AND tn.tenant_id = @tenant_id
                """;
            command.Parameters.Add("url_path", NpgsqlTypes.NpgsqlDbType.Varchar);
            command.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
            await command.PrepareAsync();
            command.Parameters["url_path"].Value = urlPath[1..];
            command.Parameters["tenant_id"].Value = tenantId;

            var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync()) {
                return reader.GetString(0);
            }
            return null;
        });
    }

}

