namespace PoundPupLegacy.Db;

public class SubgroupCreator : IEntityCreator<Subgroup>
{
    public static async Task CreateAsync(IAsyncEnumerable<Subgroup> subgroups, NpgsqlConnection connection)
    {

        await using var userGroupWriter = await UserGroupWriter.CreateAsync(connection);
        await using var publishingUserGroupWriter = await PublishingUserGroupWriter.CreateAsync(connection);
        await using var subgroupWriter = await SubgroupWriter.CreateAsync(connection);
        await using var principalWriter = await PrincipalWriter.CreateAsync(connection);
        await using var userRoleWriter = await UserRoleWriter.CreateAsync(connection);
        await using var accessRoleWriter = await AccessRoleWriter.CreateAsync(connection);
        await using var administratorRoleWriter = await AdministratorRoleWriter.CreateAsync(connection);


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
