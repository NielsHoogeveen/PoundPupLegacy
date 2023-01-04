namespace PoundPupLegacy.Db;

public class UserGroupCreator : IEntityCreator<UserGroup>
{
    public static async Task CreateAsync(IEnumerable<UserGroup> userGroups, NpgsqlConnection connection)
    {

        await using var accessRoleWriter = await AccessRoleWriter.CreateAsync(connection);
        await using var userGroupWriter = await UserGroupWriter.CreateAsync(connection);

        foreach (var userGroup in userGroups)
        {
            await accessRoleWriter.WriteAsync(userGroup);
            await userGroupWriter.WriteAsync(userGroup);
        }
    }
}
