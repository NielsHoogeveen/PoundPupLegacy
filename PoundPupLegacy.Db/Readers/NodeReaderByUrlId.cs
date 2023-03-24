using System.Data;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.Db.Readers;

public sealed class NodeReaderByUrlId : DatabaseReader, IDatabaseReader<NodeReaderByUrlId>
{
    public static async Task<NodeReaderByUrlId> CreateAsync(NpgsqlConnection connection)
    {
        var sql = """
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

        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;

        command.Parameters.Add("tenant_id", NpgsqlDbType.Integer);
        command.Parameters.Add("url_id", NpgsqlDbType.Integer);
        await command.PrepareAsync();

        return new NodeReaderByUrlId(command);

    }

    internal NodeReaderByUrlId(NpgsqlCommand command) : base(command) { }

    public async Task<Node> ReadAsync(int tenantId, int urlId)
    {
        _command.Parameters["tenant_id"].Value = tenantId;
        _command.Parameters["url_id"].Value = urlId;

        var reader = await _command.ExecuteReaderAsync();
        if (reader.HasRows) {
            await reader.ReadAsync();
            var node = new BasicNode {
                Id = reader.GetInt32("id"),
                PublisherId = reader.GetInt32("publisher_id"),
                Title = reader.GetString("title"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                NodeTypeId = reader.GetInt32("node_type_id"),
                OwnerId = reader.GetInt32("owner_id"),
                TenantNodes = new List<TenantNode>(),
            };
            await reader.CloseAsync();
            return node;
        }
        await reader.CloseAsync();
        var error = $"node cannot be found in for url_id {urlId} and tenant {tenantId}";
        throw new Exception(error);
    }
}

