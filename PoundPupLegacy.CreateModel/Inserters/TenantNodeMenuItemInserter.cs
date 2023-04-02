namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class TenantNodeMenuItemInserter : DatabaseInserter<TenantNodeMenuItem>, IDatabaseInserter<TenantNodeMenuItem>
{

    private const string ID = "id";
    private const string NAME = "name";
    private const string TENANT_NODE_ID = "tenant_node_id";
    public static async Task<DatabaseInserter<TenantNodeMenuItem>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "tenant_node_menu_item",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = TENANT_NODE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new TenantNodeMenuItemInserter(command);

    }

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
