namespace PoundPupLegacy.CreateModel.Creators;

public class AccessRoleCreator : IEntityCreator<AccessRole>
{
    public async Task CreateAsync(IAsyncEnumerable<AccessRole> accessRoles, IDbConnection connection)
    {

        await using var principalWriter = await PrincipalInserter.CreateAsync(connection);
        await using var userRoleWriter = await UserRoleInserter.CreateAsync(connection);
        await using var accessRoleWriter = await AccessRoleInserter.CreateAsync(connection);

        await foreach (var accessRole in accessRoles) {
            await principalWriter.InsertAsync(accessRole);
            await userRoleWriter.InsertAsync(accessRole);
            await accessRoleWriter.InsertAsync(accessRole);
        }
    }
}
