namespace PoundPupLegacy.Db;

public class UserGroupUserCreator : IEntityCreator<UserGroupUser>
{
    public static void Create(IEnumerable<UserGroupUser> userGroupUsers, NpgsqlConnection connection)
    {

        using var userGroupUserWriter = UserGroupUserWriter.Create(connection);

        foreach (var userGroupUser in userGroupUsers)
        {
            userGroupUserWriter.Write(userGroupUser);
        }
    }
}
