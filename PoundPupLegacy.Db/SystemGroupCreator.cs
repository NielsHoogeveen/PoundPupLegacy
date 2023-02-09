namespace PoundPupLegacy.Db;

public class SystemGroupCreator
{
    public static async Task CreateAsync(NpgsqlConnection connection)
    {

        await using var userGroupWriter = await UserGroupWriter.CreateAsync(connection);
        await using var systemGroupWriter = await SystemGroupWriter.CreateAsync(connection);
        await using var principalWriter = await PrincipalWriter.CreateAsync(connection);
        await using var userRoleWriter = await UserRoleWriter.CreateAsync(connection);
        await using var accessRoleWriter = await AccessRoleWriter.CreateAsync(connection);
        await using var administratorRoleWriter = await AdministratorRoleWriter.CreateAsync(connection);

        var systemGroup = new SystemGroup();
        await userGroupWriter.WriteAsync(systemGroup);
        await systemGroupWriter.WriteAsync(systemGroup);

        var administratorRole = systemGroup.AdministratorRole;
        administratorRole.UserGroupId = systemGroup.Id!.Value;
        await principalWriter.WriteAsync(administratorRole);
        await userRoleWriter.WriteAsync(administratorRole);
        await administratorRoleWriter.WriteAsync(administratorRole);

    }
}
