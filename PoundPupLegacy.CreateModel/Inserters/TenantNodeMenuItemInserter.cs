namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class TenantNodeMenuItemInserterFactory : DatabaseInserterFactory<TenantNodeMenuItem, TenantNodeMenuItemInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableIntegerDatabaseParameter TenantNodeId = new() { Name = "tenant_node_id" };

    public override string TableName => "tenant_node_menu_item";
}
internal sealed class TenantNodeMenuItemInserter : DatabaseInserter<TenantNodeMenuItem>
{
    public TenantNodeMenuItemInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(TenantNodeMenuItem item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(TenantNodeMenuItemInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(TenantNodeMenuItemInserterFactory.Name, item.Name),
            ParameterValue.Create(TenantNodeMenuItemInserterFactory.TenantNodeId, item.TenantNodeId),
        };
    }
}
