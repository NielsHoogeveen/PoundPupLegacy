namespace PoundPupLegacy.CreateModel.Creators;

public class SystemGroupCreator
{
    public static async Task CreateAsync(NpgsqlConnection connection)
    {

        await using var userGroupWriter = await UserGroupInserter.CreateAsync(connection);
        await using var systemGroupWriter = await SystemGroupInserter.CreateAsync(connection);
        await using var principalWriter = await PrincipalInserter.CreateAsync(connection);
        await using var userRoleWriter = await UserRoleInserter.CreateAsync(connection);
        await using var accessRoleWriter = await AccessRoleInserter.CreateAsync(connection);
        await using var administratorRoleWriter = await AdministratorRoleInserter.CreateAsync(connection);

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
