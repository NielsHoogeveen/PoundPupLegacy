namespace PoundPupLegacy.Db;

public class UserGroupUserRoleUserCreator : IEntityCreator<Model.UserGroupUserRoleUser>
{
    public static async Task CreateAsync(IAsyncEnumerable<Model.UserGroupUserRoleUser> userGroupUserRoleUsers, NpgsqlConnection connection)
    {

        await using var userGroupUserRoleUserWriter = await UserGroupUserRoleUserWriter.CreateAsync(connection);

        await foreach (var userGroupUserRoleUser in userGroupUserRoleUsers) {
            await userGroupUserRoleUserWriter.WriteAsync(userGroupUserRoleUser);
        }
    }
}
