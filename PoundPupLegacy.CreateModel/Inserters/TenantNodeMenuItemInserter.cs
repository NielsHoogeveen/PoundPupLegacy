namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = TenantNodeMenuItemInserterFactory;
using Request = TenantNodeMenuItem;
using Inserter = TenantNodeMenuItemInserter;

internal sealed class TenantNodeMenuItemInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableIntegerDatabaseParameter TenantNodeId = new() { Name = "tenant_node_id" };

    public override string TableName => "tenant_node_menu_item";
}
internal sealed class TenantNodeMenuItemInserter : IdentifiableDatabaseInserter<Request>
{
    public TenantNodeMenuItemInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.Name, request.Name),
            ParameterValue.Create(Factory.TenantNodeId, request.TenantNodeId),
        };
    }
}
