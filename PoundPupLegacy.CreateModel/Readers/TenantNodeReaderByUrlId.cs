﻿namespace PoundPupLegacy.CreateModel.Readers;
public sealed class TenantNodeReaderByUrlIdFactory : IDatabaseReaderFactory<TenantNodeReaderByUrlId>
{
    public async Task<TenantNodeReaderByUrlId> CreateAsync(NpgsqlConnection connection)
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

}
public sealed class TenantNodeReaderByUrlId : SingleItemDatabaseReader<TenantNodeReaderByUrlId.Request, TenantNode?>
{
    public record Request
    {
        public int TenantId { get; init; }
        public int UrlId { get; init; }

    }

    internal TenantNodeReaderByUrlId(NpgsqlCommand command) : base(command) { }

    public override async Task<TenantNode?> ReadAsync(Request request)
    {

        _command.Parameters["tenant_id"].Value = request.TenantId;
        _command.Parameters["url_id"].Value = request.UrlId;

        var reader = await _command.ExecuteReaderAsync();
        if (reader.HasRows) {
            await reader.ReadAsync();
            var tenantNode = new TenantNode {
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
        else {
            await reader.CloseAsync();
            return null;
        }
    }
}
