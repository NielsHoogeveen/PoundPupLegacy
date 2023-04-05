namespace PoundPupLegacy.CreateModel.Inserters;
public sealed class TenantNodeInserterFactory : DatabaseInserterFactory<TenantNode>
{
    public override async Task<IDatabaseInserter<TenantNode>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var collumnDefinitions = new ColumnDefinition[]
        {
            new ColumnDefinition{
                Name = TenantNodeInserter.TENANT_ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new ColumnDefinition{
                Name = TenantNodeInserter.URL_ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new ColumnDefinition{
                Name = TenantNodeInserter.URL_PATH,
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
            new ColumnDefinition{
                Name = TenantNodeInserter.NODE_ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new ColumnDefinition{
                Name = TenantNodeInserter.SUBGROUP_ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new ColumnDefinition{
                Name = TenantNodeInserter.PUBLICATION_STATUS_ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
        };

        var command = await CreateIdentityInsertStatementAsync(
            postgresConnection,
            "tenant_node",
            collumnDefinitions
        );
        return new TenantNodeInserter(command);
    }
}
public sealed class TenantNodeInserter : DatabaseInserter<TenantNode>
{

    internal const string TENANT_ID = "tenant_id";
    internal const string URL_ID = "url_id";
    internal const string URL_PATH = "url_path";
    internal const string NODE_ID = "node_id";
    internal const string SUBGROUP_ID = "subgroup_id";
    internal const string PUBLICATION_STATUS_ID = "publication_status_id";


    internal TenantNodeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(TenantNode tenantNode)
    {
        if (tenantNode.Id != null) {
            throw new Exception($"Id of tenant node needs to be null");
        }
        WriteValue(tenantNode.TenantId, TENANT_ID);
        WriteValue(tenantNode.UrlId.HasValue ? tenantNode.UrlId.Value : tenantNode.NodeId, URL_ID);
        WriteNullableValue(tenantNode.UrlPath?.Trim(), URL_PATH);
        WriteValue(tenantNode.NodeId, NODE_ID);
        WriteNullableValue(tenantNode.SubgroupId, SUBGROUP_ID);
        WriteValue(tenantNode.PublicationStatusId, PUBLICATION_STATUS_ID);
        tenantNode.Id = await _command.ExecuteScalarAsync() switch {
            long i => (int)i,
            _ => throw new Exception("No id was generated for tenant node")
        };
    }
}
