namespace PoundPupLegacy.CreateModel.Creators;

public class UserGroupUserRoleUserCreator : IEntityCreator<UserGroupUserRoleUser>
{
    public static async Task CreateAsync(IAsyncEnumerable<UserGroupUserRoleUser> userGroupUserRoleUsers, NpgsqlConnection connection)
    {

        await using var userGroupUserRoleUserWriter = await UserGroupUserRoleUserInserter.CreateAsync(connection);

        await foreach (var userGroupUserRoleUser in userGroupUserRoleUsers) {
            await userGroupUserRoleUserWriter.WriteAsync(userGroupUserRoleUser);
        }
    }
}
