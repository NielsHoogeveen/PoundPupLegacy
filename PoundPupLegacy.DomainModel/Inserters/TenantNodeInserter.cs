namespace PoundPupLegacy.DomainModel.Inserters;

using Request = TenantNode.ToCreate.ForExistingNode;

public sealed class TenantNodeInserterFactory : AutoGenerateIdDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UrlId = new() { Name = "url_id" };
    private static readonly TrimmingNullableStringDatabaseParameter UrlPath = new() { Name = "url_path" };
    private static readonly NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    private static readonly NullableIntegerDatabaseParameter SubgroupId = new() { Name = "subgroup_id" };
    private static readonly NonNullableIntegerDatabaseParameter PublicationStatusId = new() { Name = "publication_status_id" };

    public override string TableName => "tenant_node";
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantId, request.TenantId),
            ParameterValue.Create(UrlId, request.UrlId),
            ParameterValue.Create(UrlPath, request.UrlPath),
            ParameterValue.Create(NodeId, request.NodeId),
            ParameterValue.Create(SubgroupId, request.SubgroupId),
            ParameterValue.Create(PublicationStatusId, request.PublicationStatusId)
        };
    }
}
