namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class TenantCreator : IEntityCreator<Tenant>
{
    public async Task CreateAsync(IAsyncEnumerable<Tenant> tenants, IDbConnection connection)
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
            await userGroupWriter.InsertAsync(tenant);
            await ownerWriter.InsertAsync(tenant);
            await publishingUserGroupWriter.InsertAsync(tenant);
            await tenantWriter.InsertAsync(tenant);

            var accessRole = tenant.AccessRoleNotLoggedIn;
            accessRole.UserGroupId = tenant.Id!.Value;
            await principalWriter.InsertAsync(accessRole);
            await userRoleWriter.InsertAsync(accessRole);
            await accessRoleWriter.InsertAsync(accessRole);


            var administratorRole = tenant.AdministratorRole;
            administratorRole.UserGroupId = tenant.Id.Value;
            await principalWriter.InsertAsync(administratorRole);
            await userRoleWriter.InsertAsync(administratorRole);
            await administratorRoleWriter.InsertAsync(administratorRole);

        }
    }
}
