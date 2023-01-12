namespace PoundPupLegacy.Db;

public class TenantCreator : IEntityCreator<Tenant>
{
    public static async Task CreateAsync(IAsyncEnumerable<Tenant> tenants, NpgsqlConnection connection)
    {

        await using var userGroupWriter = await UserGroupWriter.CreateAsync(connection);
        await using var ownerWriter = await OwnerWriter.CreateAsync(connection);
        await using var tenantWriter = await TenantWriter.CreateAsync(connection);

        await foreach (var tenant in tenants)
        {
            await userGroupWriter.WriteAsync(tenant);
            await ownerWriter.WriteAsync(tenant);
            await tenantWriter.WriteAsync(tenant);
        }
    }
}
