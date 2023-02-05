namespace PoundPupLegacy.Db;

public class TenantCreator : IEntityCreator<Tenant>
{
    public static async Task CreateAsync(IAsyncEnumerable<Tenant> tenants, NpgsqlConnection connection)
    {

        await using var userGroupWriter = await UserGroupWriter.CreateAsync(connection);
        await using var ownerWriter = await OwnerWriter.CreateAsync(connection);
        await using var tenantWriter = await TenantWriter.CreateAsync(connection);
        await using var principalWriter = await PrincipalWriter.CreateAsync(connection);
        await using var accessRoleWriter = await AccessRoleWriter.CreateAsync(connection);
        await using var userRoleWriter = await UserRoleWriter.CreateAsync(connection);

        await foreach (var tenant in tenants)
        {
            await userGroupWriter.WriteAsync(tenant);
            await ownerWriter.WriteAsync(tenant);
            await tenantWriter.WriteAsync(tenant);

            var userRole = tenant.UserRoleNotLoggedIn;
            userRole.UserGroupId = tenant.Id;
            await principalWriter.WriteAsync(userRole);
            await accessRoleWriter.WriteAsync(userRole);
            await userRoleWriter.WriteAsync(userRole);
        }
    }
}
