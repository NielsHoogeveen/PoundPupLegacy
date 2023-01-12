namespace PoundPupLegacy.Db;

public class AnonimousUserCreator
{
    public static async Task CreateAsync(NpgsqlConnection connection)
    {

        await using var principalWriter = await PrincipalWriter.CreateAsync(connection);
        await using var accessRoleWriter = await AccessRoleWriter.CreateAsync(connection);

        await principalWriter.WriteAsync(new AnonymousUser());
        await accessRoleWriter.WriteAsync(new AnonymousUser());
    }
}
