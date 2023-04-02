namespace PoundPupLegacy.CreateModel.Creators;

public class SubgroupCreator : IEntityCreator<Subgroup>
{
    public async Task CreateAsync(IAsyncEnumerable<Subgroup> subgroups, IDbConnection connection)
    {

        await using var userGroupWriter = await UserGroupInserter.CreateAsync(connection);
        await using var publishingUserGroupWriter = await PublishingUserGroupInserter.CreateAsync(connection);
        await using var subgroupWriter = await SubgroupInserter.CreateAsync(connection);
        await using var principalWriter = await PrincipalInserter.CreateAsync(connection);
        await using var userRoleWriter = await UserRoleInserter.CreateAsync(connection);
        await using var accessRoleWriter = await AccessRoleInserter.CreateAsync(connection);
        await using var administratorRoleWriter = await AdministratorRoleInserter.CreateAsync(connection);


        await foreach (var subgroup in subgroups) {
            await userGroupWriter.InsertAsync(subgroup);
            await publishingUserGroupWriter.InsertAsync(subgroup);
            await subgroupWriter.InsertAsync(subgroup);

            var administratorRole = subgroup.AdministratorRole;
            administratorRole.UserGroupId = subgroup.Id.Value;
            await principalWriter.InsertAsync(administratorRole);
            await userRoleWriter.InsertAsync(administratorRole);
            await administratorRoleWriter.InsertAsync(administratorRole);
        }
    }
}
