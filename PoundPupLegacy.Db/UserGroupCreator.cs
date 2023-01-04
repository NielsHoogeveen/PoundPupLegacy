namespace PoundPupLegacy.Db;

public class UserGroupCreator : IEntityCreator<UserGroup>
{
    public static async Task CreateAsync(IAsyncEnumerable<UserGroup> userGroups, NpgsqlConnection connection)
    {

        await using var accessRoleWriter = await AccessRoleWriter.CreateAsync(connection);
        await using var userGroupWriter = await UserGroupWriter.CreateAsync(connection);

        await foreach (var userGroup in userGroups)
        {
            await accessRoleWriter.WriteAsync(userGroup);
            await userGroupWriter.WriteAsync(userGroup);
        }
    }
}
