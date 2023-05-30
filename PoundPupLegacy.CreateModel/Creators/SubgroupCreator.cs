namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SubgroupCreatorFactory(
    IDatabaseInserterFactory<Subgroup> subgroupInserterFactory,
    IDatabaseInserterFactory<UserGroup> userGroupInserterFactory,
    IDatabaseInserterFactory<PublishingUserGroup> publishingUserGroupInserterFactory,
    IDatabaseInserterFactory<Principal> principalInserterFactory,
    IDatabaseInserterFactory<UserRole> userRoleInserterFactory,
    IDatabaseInserterFactory<AccessRole> accessRoleInserterFactory,
    IDatabaseInserterFactory<AdministratorRole> administratorRoleInserterFactory
) : IEntityCreatorFactory<Subgroup>
{
    public async Task<IEntityCreator<Subgroup>> CreateAsync(IDbConnection connection) =>
        new SubgroupCreator(
            new() {
                await userGroupInserterFactory.CreateAsync(connection),
                await publishingUserGroupInserterFactory.CreateAsync(connection),
                await subgroupInserterFactory.CreateAsync(connection)
            },
            await principalInserterFactory.CreateAsync(connection),
            await userRoleInserterFactory.CreateAsync(connection),
            await accessRoleInserterFactory.CreateAsync(connection),
            await administratorRoleInserterFactory.CreateAsync(connection)
        );
}


internal class SubgroupCreator(
    List<IDatabaseInserter<Subgroup>> inserters,
    IDatabaseInserter<Principal> principalInserter,
    IDatabaseInserter<UserRole> userRoleInserter,
    IDatabaseInserter<AccessRole> accessRoleInserter,
    IDatabaseInserter<AdministratorRole> administratorRoleInserter

) : InsertingEntityCreator<Subgroup>(inserters) 
{
    public override async Task ProcessAsync(Subgroup element)
    {
        await base.ProcessAsync(element);
        var administratorRole = element.AdministratorRole;
        administratorRole.UserGroupId = element.IdentificationForCreate.Id;
        await principalInserter.InsertAsync(administratorRole);
        await userRoleInserter.InsertAsync(administratorRole);
        await administratorRoleInserter.InsertAsync(administratorRole);

    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await principalInserter.DisposeAsync();
        await userRoleInserter.DisposeAsync();
        await accessRoleInserter.DisposeAsync();
        await administratorRoleInserter.DisposeAsync();
    }
}