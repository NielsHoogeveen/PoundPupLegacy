namespace PoundPupLegacy.CreateModel.Inserters;
public sealed class TenantNodeInserterFactory : AutoGenerateIdDatabaseInserterFactory<TenantNode, TenantNodeInserter>
{
    internal static NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    internal static NonNullableIntegerDatabaseParameter UrlId = new() { Name = "url_id" };
    internal static NullableStringDatabaseParameter UrlPath = new() { Name = "url_path" };
    internal static NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    internal static NullableIntegerDatabaseParameter SubgroupId = new() { Name = "subgroup_id" };
    internal static NonNullableIntegerDatabaseParameter PublicationStatusId = new() { Name = "publication_status_id" };

    public override string TableName => "tenant_node";

}
public sealed class TenantNodeInserter : AutoGenerateIdDatabaseInserter<TenantNode>
{

    public TenantNodeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(TenantNode tenantNode)
    {
        if (tenantNode.Id.HasValue) {
            throw new Exception($"tenant node id should be null upon creation");
        }
        if (tenantNode.NodeId is null)
            throw new NullReferenceException(nameof(tenantNode.NodeId));
        return new ParameterValue[] {
            ParameterValue.Create(TenantNodeInserterFactory.TenantId, tenantNode.TenantId),
            ParameterValue.Create(TenantNodeInserterFactory.UrlId, tenantNode.UrlId.HasValue ? tenantNode.UrlId.Value : tenantNode.NodeId.Value),
            ParameterValue.Create(TenantNodeInserterFactory.UrlPath, tenantNode.UrlPath?.Trim()),
            ParameterValue.Create(TenantNodeInserterFactory.NodeId, tenantNode.NodeId.Value),
            ParameterValue.Create(TenantNodeInserterFactory.SubgroupId, tenantNode.SubgroupId),
            ParameterValue.Create(TenantNodeInserterFactory.PublicationStatusId, tenantNode.PublicationStatusId)
        };
    }
}
