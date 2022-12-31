namespace PoundPupLegacy.Db;

public class AnonimousUserCreator
{
    public static void Create(NpgsqlConnection connection)
    {

        using var accessRoleWriter = AccessRoleWriter.Create(connection);

        accessRoleWriter.Write(new AnonymousUser());
    }
}
