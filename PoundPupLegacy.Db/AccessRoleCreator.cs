namespace PoundPupLegacy.Db;

public class AccessRoleCreator : IEntityCreator<AccessRole>
{
    public static async Task CreateAsync(IAsyncEnumerable<AccessRole> accessRoles, NpgsqlConnection connection)
    {

        await using var principalWriter = await PrincipalWriter.CreateAsync(connection);
        await using var userRoleWriter = await UserRoleWriter.CreateAsync(connection);
        await using var accessRoleWriter = await AccessRoleWriter.CreateAsync(connection);

        await foreach (var accessRole in accessRoles) {
            await principalWriter.WriteAsync(accessRole);
            await userRoleWriter.WriteAsync(accessRole);
            await accessRoleWriter.WriteAsync(accessRole);
        }
    }
}
