namespace PoundPupLegacy.CreateModel.Inserters;

using Request = TenantNode;

public sealed class TenantNodeInserterFactory : AutoGenerateIdDatabaseInserterFactory<Request>
{
    internal static NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    internal static NullCheckingAlternativeIntegerDatabaseParameter UrlId = new() { Name = "url_id" };
    internal static TrimmingNullableStringDatabaseParameter UrlPath = new() { Name = "url_path" };
    internal static NullCheckingIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    internal static NullableIntegerDatabaseParameter SubgroupId = new() { Name = "subgroup_id" };
    internal static NonNullableIntegerDatabaseParameter PublicationStatusId = new() { Name = "publication_status_id" };

    public override string TableName => "tenant_node";
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantId, request.TenantId),
            ParameterValue.Create(UrlId, (request.UrlId, request.NodeId)),
            ParameterValue.Create(UrlPath, request.UrlPath),
            ParameterValue.Create(NodeId, request.NodeId),
            ParameterValue.Create(SubgroupId, request.SubgroupId),
            ParameterValue.Create(PublicationStatusId, request.PublicationStatusId)
        };
    }
}
