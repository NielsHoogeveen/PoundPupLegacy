namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class TenantNodeMenuItemInserterFactory : DatabaseInserterFactory<TenantNodeMenuItem>
{
    public override async Task<IDatabaseInserter<TenantNodeMenuItem>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "tenant_node_menu_item",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = TenantNodeMenuItemInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = TenantNodeMenuItemInserter.NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = TenantNodeMenuItemInserter.TENANT_NODE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new TenantNodeMenuItemInserter(command);
    }
}
internal sealed class TenantNodeMenuItemInserter : DatabaseInserter<TenantNodeMenuItem>
{

    internal const string ID = "id";
    internal const string NAME = "name";
    internal const string TENANT_NODE_ID = "tenant_node_id";

    internal TenantNodeMenuItemInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(TenantNodeMenuItem tenantNodeMenuItem)
    {
        WriteValue(tenantNodeMenuItem.Id, ID);
        WriteValue(tenantNodeMenuItem.Name, NAME);
        WriteValue(tenantNodeMenuItem.TenantNodeId, TENANT_NODE_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
