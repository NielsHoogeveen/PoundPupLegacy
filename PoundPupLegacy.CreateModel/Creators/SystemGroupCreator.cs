namespace PoundPupLegacy.CreateModel.Creators;

public class SystemGroupCreator
{
    public async Task CreateAsync(IDbConnection connection)
    {

        await using var userGroupWriter = await UserGroupInserter.CreateAsync(connection);
        await using var systemGroupWriter = await SystemGroupInserter.CreateAsync(connection);
        await using var principalWriter = await PrincipalInserter.CreateAsync(connection);
        await using var userRoleWriter = await UserRoleInserter.CreateAsync(connection);
        await using var accessRoleWriter = await AccessRoleInserter.CreateAsync(connection);
        await using var administratorRoleWriter = await AdministratorRoleInserter.CreateAsync(connection);

        var systemGroup = new SystemGroup();
        await userGroupWriter.InsertAsync(systemGroup);
        await systemGroupWriter.InsertAsync(systemGroup);

        var administratorRole = systemGroup.AdministratorRole;
        administratorRole.UserGroupId = systemGroup.Id!.Value;
        await principalWriter.InsertAsync(administratorRole);
        await userRoleWriter.InsertAsync(administratorRole);
        await administratorRoleWriter.InsertAsync(administratorRole);

    }
}
