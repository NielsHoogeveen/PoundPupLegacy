namespace PoundPupLegacy.Db;

public class TenantCreator : IEntityCreator<Tenant>
{
    public static async Task CreateAsync(IAsyncEnumerable<Tenant> tenants, NpgsqlConnection connection)
    {

        await using var userGroupWriter = await UserGroupWriter.CreateAsync(connection);
        await using var ownerWriter = await OwnerWriter.CreateAsync(connection);
        await using var tenantWriter = await TenantWriter.CreateAsync(connection);
        await using var principalWriter = await PrincipalWriter.CreateAsync(connection);
        await using var userRoleWriter = await UserRoleWriter.CreateAsync(connection);
        await using var accessRoleWriter = await AccessRoleWriter.CreateAsync(connection);
        await using var administratorRoleWriter = await AdministratorRoleWriter.CreateAsync(connection);

        await foreach (var tenant in tenants) {
            await userGroupWriter.WriteAsync(tenant);
            await ownerWriter.WriteAsync(tenant);
            await tenantWriter.WriteAsync(tenant);

            var accessRole = tenant.AccessRoleNotLoggedIn;
            accessRole.UserGroupId = tenant.Id!.Value;
            await principalWriter.WriteAsync(accessRole);
            await userRoleWriter.WriteAsync(accessRole);
            await accessRoleWriter.WriteAsync(accessRole);


            var administratorRole = tenant.AdministratorRole;
            administratorRole.UserGroupId = tenant.Id.Value;
            await principalWriter.WriteAsync(administratorRole);
            await userRoleWriter.WriteAsync(administratorRole);
            await administratorRoleWriter.WriteAsync(administratorRole);

        }
    }
}
