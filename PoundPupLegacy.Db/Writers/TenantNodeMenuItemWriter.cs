namespace PoundPupLegacy.Db.Writers;

internal sealed class TenantNodeMenuItemWriter : DatabaseWriter<TenantNodeMenuItem>, IDatabaseWriter<TenantNodeMenuItem>
{

    private const string ID = "id";
    private const string NAME = "name";
    private const string TENANT_NODE_ID = "tenant_node_id";
    public static async Task<DatabaseWriter<TenantNodeMenuItem>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
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
        return new TenantNodeMenuItemWriter(command);

    }

    internal TenantNodeMenuItemWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(TenantNodeMenuItem tenantNodeMenuItem)
    {
        WriteValue(tenantNodeMenuItem.Id, ID);
        WriteValue(tenantNodeMenuItem.Name, NAME);
        WriteValue(tenantNodeMenuItem.TenantNodeId, TENANT_NODE_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
