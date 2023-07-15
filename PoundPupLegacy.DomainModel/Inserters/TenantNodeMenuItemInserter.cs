namespace PoundPupLegacy.DomainModel.Inserters;

using Request = TenantNodeMenuItem;

internal sealed class TenantNodeMenuItemInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    private static readonly NonNullableIntegerDatabaseParameter TenantNodeId = new() { Name = "tenant_node_id" };

    public override string TableName => "tenant_node_menu_item";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Name, request.Name),
            ParameterValue.Create(TenantNodeId, request.TenantNodeId),
        };
    }
}
