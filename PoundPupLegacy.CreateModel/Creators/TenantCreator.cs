namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class TenantCreatorFactory(
    IDatabaseInserterFactory<UserGroup> userGroupInserterFactory,
    IDatabaseInserterFactory<Owner> ownerInserterFactory,
    IDatabaseInserterFactory<PublishingUserGroup> publishingUserGroupInserterFactory,
    IDatabaseInserterFactory<Tenant> tenantInserterFactory,
    IDatabaseInserterFactory<AccessRole> accessRoleInserterFactory,
    IDatabaseInserterFactory<Principal> principalInserterFactory,
    IDatabaseInserterFactory<UserRole> userRoleInserterFactory,
    IDatabaseInserterFactory<AdministratorRole> administratorRoleInserterFactory
) : IEntityCreatorFactory<Tenant>
{
    public async Task<IEntityCreator<Tenant>> CreateAsync(IDbConnection connection) =>
        new TenantCreator(
            new() 
            {
                await userGroupInserterFactory.CreateAsync(connection),
                await ownerInserterFactory.CreateAsync(connection),
                await publishingUserGroupInserterFactory.CreateAsync(connection),
                await tenantInserterFactory.CreateAsync(connection)
            },
            await principalInserterFactory.CreateAsync(connection),
            await accessRoleInserterFactory.CreateAsync(connection),
            await userRoleInserterFactory.CreateAsync(connection),
            await administratorRoleInserterFactory.CreateAsync(connection)
        );
}
internal class TenantCreator(
    List<IDatabaseInserter<Tenant>> inserters,
    IDatabaseInserter<Principal> principalInserter,
    IDatabaseInserter<AccessRole> accessRoleInserter,
    IDatabaseInserter<UserRole> userRoleInserter,
    IDatabaseInserter<AdministratorRole> administratorRoleInserter
) : InsertingEntityCreator<Tenant>(inserters)
{
    public override async Task ProcessAsync(Tenant element)
    {
        await base.ProcessAsync(element);

        var accessRole = element.AccessRoleNotLoggedIn;
        accessRole.UserGroupId = element.IdentificationForCreate.Id!.Value;
        await principalInserter.InsertAsync(accessRole);
        await userRoleInserter.InsertAsync(accessRole);
        await accessRoleInserter.InsertAsync(accessRole);

        var administratorRole = element.AdministratorRole;
        administratorRole.UserGroupId = element.IdentificationForCreate.Id.Value;
        await principalInserter.InsertAsync(administratorRole);
        await userRoleInserter.InsertAsync(administratorRole);
        await administratorRoleInserter.InsertAsync(administratorRole);

    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await accessRoleInserter.DisposeAsync();
        await principalInserter.DisposeAsync();
        await userRoleInserter.DisposeAsync();
        await administratorRoleInserter.DisposeAsync();
    }
}