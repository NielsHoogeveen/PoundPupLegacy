namespace PoundPupLegacy.CreateModel.Creators;

public class AccessRoleCreator : IEntityCreator<AccessRole>
{
    public static async Task CreateAsync(IAsyncEnumerable<AccessRole> accessRoles, NpgsqlConnection connection)
    {

        await using var principalWriter = await PrincipalInserter.CreateAsync(connection);
        await using var userRoleWriter = await UserRoleInserter.CreateAsync(connection);
        await using var accessRoleWriter = await AccessRoleInserter.CreateAsync(connection);

        await foreach (var accessRole in accessRoles) {
            await principalWriter.WriteAsync(accessRole);
            await userRoleWriter.WriteAsync(accessRole);
            await accessRoleWriter.WriteAsync(accessRole);
        }
    }
}
