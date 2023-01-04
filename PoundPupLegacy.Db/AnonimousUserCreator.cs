namespace PoundPupLegacy.Db;

public class AnonimousUserCreator
{
    public static async Task CreateAsync(NpgsqlConnection connection)
    {

        await using var accessRoleWriter = await AccessRoleWriter.CreateAsync(connection);

        await accessRoleWriter.WriteAsync(new AnonymousUser());
    }
}
