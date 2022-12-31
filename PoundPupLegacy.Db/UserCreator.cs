namespace PoundPupLegacy.Db;

public class UserCreator : IEntityCreator<User>
{
    public static void Create(IEnumerable<User> users, NpgsqlConnection connection)
    {

        using var accessRoleWriter = AccessRoleWriter.Create(connection);
        using var userWriter = UserWriter.Create(connection);

        foreach (var user in users)
        {
            accessRoleWriter.Write(user);
            userWriter.Write(user);
        }
    }
}