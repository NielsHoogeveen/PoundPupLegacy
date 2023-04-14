namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = TenantNodeInserterFactory;
using Request = TenantNode;
using Inserter = TenantNodeInserter;
public sealed class TenantNodeInserterFactory : AutoGenerateIdDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    internal static NullCheckingAlternativeIntegerDatabaseParameter UrlId = new() { Name = "url_id" };
    internal static TrimmingNullableStringDatabaseParameter UrlPath = new() { Name = "url_path" };
    internal static NullCheckingIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    internal static NullableIntegerDatabaseParameter SubgroupId = new() { Name = "subgroup_id" };
    internal static NonNullableIntegerDatabaseParameter PublicationStatusId = new() { Name = "publication_status_id" };

    public override string TableName => "tenant_node";

}
public sealed class TenantNodeInserter : AutoGenerateIdDatabaseInserter<Request>
{

    public TenantNodeInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.TenantId, request.TenantId),
            ParameterValue.Create(Factory.UrlId, (request.UrlId, request.NodeId)),
            ParameterValue.Create(Factory.UrlPath, request.UrlPath),
            ParameterValue.Create(Factory.NodeId, request.NodeId),
            ParameterValue.Create(Factory.SubgroupId, request.SubgroupId),
            ParameterValue.Create(Factory.PublicationStatusId, request.PublicationStatusId)
        };
    }
}
