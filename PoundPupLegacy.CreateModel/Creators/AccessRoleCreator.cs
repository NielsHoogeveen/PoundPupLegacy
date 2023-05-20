namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class AccessRoleCreator(
    IDatabaseInserterFactory<Principal> principalInserterFactory,
    IDatabaseInserterFactory<UserRole> userRoleInserterFactory,
    IDatabaseInserterFactory<AccessRole> accessRoleInserterFactory
) : EntityCreator<AccessRole>
{
    public override async Task CreateAsync(IAsyncEnumerable<AccessRole> accessRoles, IDbConnection connection)
    {
        await using var principalWriter = await principalInserterFactory.CreateAsync(connection);
        await using var userRoleWriter = await userRoleInserterFactory.CreateAsync(connection);
        await using var accessRoleWriter = await accessRoleInserterFactory.CreateAsync(connection);

        await foreach (var accessRole in accessRoles) {
            await principalWriter.InsertAsync(accessRole);
            await userRoleWriter.InsertAsync(accessRole);
            await accessRoleWriter.InsertAsync(accessRole);
        }
    }
}
