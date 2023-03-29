namespace PoundPupLegacy.CreateModel.Creators;

public class TenantCreator : IEntityCreator<Tenant>
{
    public static async Task CreateAsync(IAsyncEnumerable<Tenant> tenants, NpgsqlConnection connection)
    {

        await using var userGroupWriter = await UserGroupInserter.CreateAsync(connection);
        await using var ownerWriter = await OwnerInserter.CreateAsync(connection);
        await using var publishingUserGroupWriter = await PublishingUserGroupInserter.CreateAsync(connection);
        await using var tenantWriter = await TenantInserter.CreateAsync(connection);
        await using var principalWriter = await PrincipalInserter.CreateAsync(connection);
        await using var userRoleWriter = await UserRoleInserter.CreateAsync(connection);
        await using var accessRoleWriter = await AccessRoleInserter.CreateAsync(connection);
        await using var administratorRoleWriter = await AdministratorRoleInserter.CreateAsync(connection);

        await foreach (var tenant in tenants) {
            await userGroupWriter.WriteAsync(tenant);
            await ownerWriter.WriteAsync(tenant);
            await publishingUserGroupWriter.WriteAsync(tenant);
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
