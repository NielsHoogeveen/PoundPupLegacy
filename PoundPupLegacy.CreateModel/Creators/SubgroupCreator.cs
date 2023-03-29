namespace PoundPupLegacy.CreateModel.Creators;

public class SubgroupCreator : IEntityCreator<Subgroup>
{
    public static async Task CreateAsync(IAsyncEnumerable<Subgroup> subgroups, NpgsqlConnection connection)
    {

        await using var userGroupWriter = await UserGroupInserter.CreateAsync(connection);
        await using var publishingUserGroupWriter = await PublishingUserGroupInserter.CreateAsync(connection);
        await using var subgroupWriter = await SubgroupInserter.CreateAsync(connection);
        await using var principalWriter = await PrincipalInserter.CreateAsync(connection);
        await using var userRoleWriter = await UserRoleInserter.CreateAsync(connection);
        await using var accessRoleWriter = await AccessRoleInserter.CreateAsync(connection);
        await using var administratorRoleWriter = await AdministratorRoleInserter.CreateAsync(connection);


        await foreach (var subgroup in subgroups) {
            await userGroupWriter.WriteAsync(subgroup);
            await publishingUserGroupWriter.WriteAsync(subgroup);
            await subgroupWriter.WriteAsync(subgroup);

            var administratorRole = subgroup.AdministratorRole;
            administratorRole.UserGroupId = subgroup.Id.Value;
            await principalWriter.WriteAsync(administratorRole);
            await userRoleWriter.WriteAsync(administratorRole);
            await administratorRoleWriter.WriteAsync(administratorRole);
        }
    }
}
