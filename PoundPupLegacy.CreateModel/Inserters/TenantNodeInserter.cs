namespace PoundPupLegacy.CreateModel.Inserters;
public sealed class TenantNodeInserterFactory : DatabaseInserterFactory<TenantNode>
{
    internal static NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    internal static NonNullableIntegerDatabaseParameter UrlId = new() { Name = "url_id" };
    internal static NullableStringDatabaseParameter UrlPath = new() { Name = "url_path" };
    internal static NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    internal static NullableIntegerDatabaseParameter SubgroupId = new() { Name = "subgroup_id" };
    internal static NonNullableIntegerDatabaseParameter PublicationStatusId = new() { Name = "publication_status_id" };

    public override async Task<IDatabaseInserter<TenantNode>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateAutoGenerateIdentityInsertStatementAsync(
            postgresConnection,
            "tenant_node",
            new DatabaseParameter[]
            {
                TenantId,
                UrlId,
                UrlPath,
                NodeId,
                SubgroupId,
                PublicationStatusId
            }
        );
        return new TenantNodeInserter(command);
    }
}
public sealed class TenantNodeInserter : DatabaseInserter<TenantNode>
{

    internal TenantNodeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(TenantNode tenantNode)
    {
        if (tenantNode.Id != null) {
            throw new Exception($"Id of tenant node needs to be null");
        }
        if(tenantNode.NodeId == null) {
            throw new NullReferenceException();
        }

        Set(TenantNodeInserterFactory.TenantId, tenantNode.TenantId);
        Set(TenantNodeInserterFactory.UrlId, tenantNode.UrlId.HasValue ? tenantNode.UrlId.Value : tenantNode.NodeId.Value);
        Set(TenantNodeInserterFactory.UrlPath, tenantNode.UrlPath?.Trim());
        Set(TenantNodeInserterFactory.NodeId, tenantNode.NodeId.Value);
        Set(TenantNodeInserterFactory.SubgroupId, tenantNode.SubgroupId);
        Set(TenantNodeInserterFactory.PublicationStatusId, tenantNode.PublicationStatusId);
        tenantNode.Id = await _command.ExecuteScalarAsync() switch {
            long i => (int)i,
            _ => throw new Exception("No id was generated for tenant node")
        };
    }
}
