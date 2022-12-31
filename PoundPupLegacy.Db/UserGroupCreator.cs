namespace PoundPupLegacy.Db;

public class UserGroupCreator : IEntityCreator<UserGroup>
{
    public static void Create(IEnumerable<UserGroup> userGroups, NpgsqlConnection connection)
    {

        using var accessRoleWriter = AccessRoleWriter.Create(connection);
        using var userGroupWriter = UserGroupWriter.Create(connection);

        foreach (var userGroup in userGroups)
        {
            accessRoleWriter.Write(userGroup);
            userGroupWriter.Write(userGroup);
        }
    }
}
