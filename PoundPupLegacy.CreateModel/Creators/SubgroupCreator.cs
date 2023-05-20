namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SubgroupCreator(
    IDatabaseInserterFactory<Subgroup> subgroupInserterFactory,
    IDatabaseInserterFactory<UserGroup> userGroupInserterFactory,
    IDatabaseInserterFactory<PublishingUserGroup> publishingUserGroupInserterFactory,
    IDatabaseInserterFactory<Principal> principalInserterFactory,
    IDatabaseInserterFactory<UserRole> userRoleInserterFactory,
    IDatabaseInserterFactory<AccessRole> accessRoleInserterFactory,
    IDatabaseInserterFactory<AdministratorRole> administratorRoleInserterFactory
) : EntityCreator<Subgroup>
{
    public override async Task CreateAsync(IAsyncEnumerable<Subgroup> subgroups, IDbConnection connection)
    {
        await using var userGroupWriter = await userGroupInserterFactory.CreateAsync(connection);
        await using var publishingUserGroupWriter = await publishingUserGroupInserterFactory.CreateAsync(connection);
        await using var subgroupWriter = await subgroupInserterFactory.CreateAsync(connection);
        await using var principalWriter = await principalInserterFactory.CreateAsync(connection);
        await using var userRoleWriter = await userRoleInserterFactory.CreateAsync(connection);
        await using var accessRoleWriter = await accessRoleInserterFactory.CreateAsync(connection);
        await using var administratorRoleWriter = await administratorRoleInserterFactory.CreateAsync(connection);

        await foreach (var subgroup in subgroups) {
            await userGroupWriter.InsertAsync(subgroup);
            await publishingUserGroupWriter.InsertAsync(subgroup);
            await subgroupWriter.InsertAsync(subgroup);

            var administratorRole = subgroup.AdministratorRole;
            administratorRole.UserGroupId = subgroup.Id;
            await principalWriter.InsertAsync(administratorRole);
            await userRoleWriter.InsertAsync(administratorRole);
            await administratorRoleWriter.InsertAsync(administratorRole);
        }
    }
}
