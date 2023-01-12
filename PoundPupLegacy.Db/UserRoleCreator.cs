namespace PoundPupLegacy.Db;

public class UserRoleCreator : IEntityCreator<UserRole>
{
    public static async Task CreateAsync(IAsyncEnumerable<UserRole> userRoles, NpgsqlConnection connection)
    {

        await using var principalWriter = await PrincipalWriter.CreateAsync(connection);
        await using var accessRoleWriter = await AccessRoleWriter.CreateAsync(connection);
        await using var userRoleWriter = await UserRoleWriter.CreateAsync(connection);

        await foreach (var userRole in userRoles)
        {
            await principalWriter.WriteAsync(userRole);
            await accessRoleWriter.WriteAsync(userRole);
            await userRoleWriter.WriteAsync(userRole);
        }
    }
}
