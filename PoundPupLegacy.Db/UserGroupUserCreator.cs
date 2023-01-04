namespace PoundPupLegacy.Db;

public class UserGroupUserCreator : IEntityCreator<UserGroupUser>
{
    public static async Task CreateAsync(IAsyncEnumerable<UserGroupUser> userGroupUsers, NpgsqlConnection connection)
    {

        await using var userGroupUserWriter = await UserGroupUserWriter.CreateAsync(connection);

        await foreach (var userGroupUser in userGroupUsers)
        {
            await userGroupUserWriter.WriteAsync(userGroupUser);
        }
    }
}
