namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class TenantCreator(
    IDatabaseInserterFactory<Tenant> tenantInserterFactory,
    IDatabaseInserterFactory<AccessRole> accessRoleInserterFactory,
    IDatabaseInserterFactory<Principal> principalInserterFactory,
    IDatabaseInserterFactory<UserRole> userRoleInserterFactory,
    IDatabaseInserterFactory<AdministratorRole> administratorRoleInserterFactory,
    IDatabaseInserterFactory<UserGroup> userGroupInserterFactory,
    IDatabaseInserterFactory<Owner> ownerInserterFactory,
    IDatabaseInserterFactory<PublishingUserGroup> publishingUserGroupInserterFactory
) : EntityCreator<Tenant>
{
    public override async Task CreateAsync(IAsyncEnumerable<Tenant> tenants, IDbConnection connection)
    {
        await using var userGroupWriter = await userGroupInserterFactory.CreateAsync(connection);
        await using var ownerWriter = await ownerInserterFactory.CreateAsync(connection);
        await using var publishingUserGroupWriter = await publishingUserGroupInserterFactory.CreateAsync(connection);
        await using var tenantWriter = await tenantInserterFactory.CreateAsync(connection);
        await using var principalWriter = await principalInserterFactory.CreateAsync(connection);
        await using var userRoleWriter = await userRoleInserterFactory.CreateAsync(connection);
        await using var accessRoleWriter = await accessRoleInserterFactory.CreateAsync(connection);
        await using var administratorRoleWriter = await administratorRoleInserterFactory.CreateAsync(connection);

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
