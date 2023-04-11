namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class TenantCreator : EntityCreator<Tenant>
{
    private readonly IDatabaseInserterFactory<Tenant> _tenantInserterFactory;
    private readonly IDatabaseInserterFactory<AccessRole> _accessRoleInserterFactory;
    private readonly IDatabaseInserterFactory<Principal> _principalInserterFactory;
    private readonly IDatabaseInserterFactory<UserRole> _userRoleInserterFactory;
    private readonly IDatabaseInserterFactory<AdministratorRole> _administratorRoleInserterFactory;
    private readonly IDatabaseInserterFactory<UserGroup> _userGroupInserterFactory;
    private readonly IDatabaseInserterFactory<Owner> _ownerInserterFactory;
    private readonly IDatabaseInserterFactory<PublishingUserGroup> _publishingUserGroupInserterFactory;

    public TenantCreator(
        IDatabaseInserterFactory<Tenant> tenantInserterFactory,
        IDatabaseInserterFactory<AccessRole> accessRoleInserterFactory,
        IDatabaseInserterFactory<Principal> principalInserterFactory,
        IDatabaseInserterFactory<UserRole> userRoleInserterFactory,
        IDatabaseInserterFactory<AdministratorRole> administratorRoleInserterFactory,
        IDatabaseInserterFactory<UserGroup> userGroupInserterFactory,
        IDatabaseInserterFactory<Owner> ownerInserterFactory,
        IDatabaseInserterFactory<PublishingUserGroup> publishingUserGroupInserterFactory
    )
    {
        _tenantInserterFactory = tenantInserterFactory;
        _accessRoleInserterFactory = accessRoleInserterFactory;
        _principalInserterFactory = principalInserterFactory;
        _userRoleInserterFactory = userRoleInserterFactory;
        _administratorRoleInserterFactory = administratorRoleInserterFactory;
        _userGroupInserterFactory = userGroupInserterFactory;
        _ownerInserterFactory = ownerInserterFactory;
        _publishingUserGroupInserterFactory = publishingUserGroupInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<Tenant> tenants, IDbConnection connection)
    {

        await using var userGroupWriter = await _userGroupInserterFactory.CreateAsync(connection);
        await using var ownerWriter = await _ownerInserterFactory.CreateAsync(connection);
        await using var publishingUserGroupWriter = await _publishingUserGroupInserterFactory.CreateAsync(connection);
        await using var tenantWriter = await _tenantInserterFactory.CreateAsync(connection);
        await using var principalWriter = await _principalInserterFactory.CreateAsync(connection);
        await using var userRoleWriter = await _userRoleInserterFactory.CreateAsync(connection);
        await using var accessRoleWriter = await _accessRoleInserterFactory.CreateAsync(connection);
        await using var administratorRoleWriter = await _administratorRoleInserterFactory.CreateAsync(connection);

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
