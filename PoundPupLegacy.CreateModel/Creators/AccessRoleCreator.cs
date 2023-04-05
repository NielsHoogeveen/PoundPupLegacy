namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class AccessRoleCreator : EntityCreator<AccessRole>
{
    private readonly IDatabaseInserterFactory<Principal> _principalInserterFactory;
    private readonly IDatabaseInserterFactory<UserRole> _userRoleInserterFactory;
    private readonly IDatabaseInserterFactory<AccessRole> _accessRoleInserterFactory;
    public AccessRoleCreator(
        IDatabaseInserterFactory<Principal> principalInserterFactory,
        IDatabaseInserterFactory<UserRole> userRoleInserterFactory,
        IDatabaseInserterFactory<AccessRole> accessRoleInserterFactory
        )
    {
        _principalInserterFactory = principalInserterFactory;
        _userRoleInserterFactory = userRoleInserterFactory;
        _accessRoleInserterFactory = accessRoleInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<AccessRole> accessRoles, IDbConnection connection)
    {

        await using var principalWriter = await _principalInserterFactory.CreateAsync(connection);
        await using var userRoleWriter = await _userRoleInserterFactory.CreateAsync(connection);
        await using var accessRoleWriter = await _accessRoleInserterFactory.CreateAsync(connection);

        await foreach (var accessRole in accessRoles) {
            await principalWriter.InsertAsync(accessRole);
            await userRoleWriter.InsertAsync(accessRole);
            await accessRoleWriter.InsertAsync(accessRole);
        }
    }
}
