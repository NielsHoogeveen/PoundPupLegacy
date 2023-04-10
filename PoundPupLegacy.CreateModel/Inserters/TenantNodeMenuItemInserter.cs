namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class TenantNodeMenuItemInserterFactory : DatabaseInserterFactory<TenantNodeMenuItem>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableIntegerDatabaseParameter TenantNodeId = new() { Name = "tenant_node_id" };

    public override async Task<IDatabaseInserter<TenantNodeMenuItem>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "tenant_node_menu_item",
            new DatabaseParameter[] {
                Id,
                Name,
                TenantNodeId
            }
        );
        return new TenantNodeMenuItemInserter(command);
    }
}
internal sealed class TenantNodeMenuItemInserter : DatabaseInserter<TenantNodeMenuItem>
{
    internal TenantNodeMenuItemInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(TenantNodeMenuItem tenantNodeMenuItem)
    {
        if (tenantNodeMenuItem.Id is null)
            throw new NullReferenceException();
        Set(TenantNodeMenuItemInserterFactory.Id, tenantNodeMenuItem.Id.Value);
        Set(TenantNodeMenuItemInserterFactory.Name, tenantNodeMenuItem.Name);
        Set(TenantNodeMenuItemInserterFactory.TenantNodeId, tenantNodeMenuItem.TenantNodeId);
        await _command.ExecuteNonQueryAsync();
    }
}
