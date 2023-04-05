namespace PoundPupLegacy.CreateModel.Creators;
public interface ISystemGroupCreator
{
    Task CreateAsync(IDbConnection connection);
}
internal sealed class SystemGroupCreator: ISystemGroupCreator
{
    private readonly IDatabaseInserterFactory<SystemGroup> _systemGroupInserterFactory;
    private readonly IDatabaseInserterFactory<UserGroup> _userGroupInserterFactory;
    private readonly IDatabaseInserterFactory<Principal> _principalInserterFactory;
    private readonly IDatabaseInserterFactory<UserRole> _userRoleInserterFactory;
    private readonly IDatabaseInserterFactory<AccessRole> _accessRoleInserterFactory;
    private readonly IDatabaseInserterFactory<AdministratorRole> _administratorRoleInserterFactory;
    public SystemGroupCreator(
        IDatabaseInserterFactory<SystemGroup> systemGroupInserterFactory,
        IDatabaseInserterFactory<UserGroup> userGroupInserterFactory,
        IDatabaseInserterFactory<Principal> principalInserterFactory,
        IDatabaseInserterFactory<UserRole> userRoleInserterFactory,
        IDatabaseInserterFactory<AccessRole> accessRoleInserterFactory,
        IDatabaseInserterFactory<AdministratorRole> administratorRoleInserterFactory
    )
    {
        _systemGroupInserterFactory = systemGroupInserterFactory;
        _userGroupInserterFactory = userGroupInserterFactory;
        _principalInserterFactory = principalInserterFactory;
        _userRoleInserterFactory = userRoleInserterFactory;
        _accessRoleInserterFactory = accessRoleInserterFactory;
        _administratorRoleInserterFactory = administratorRoleInserterFactory;
    }
    public async Task CreateAsync(IDbConnection connection)
    {

        await using var userGroupWriter = await _userGroupInserterFactory.CreateAsync(connection);
        await using var systemGroupWriter = await _systemGroupInserterFactory.CreateAsync(connection);
        await using var principalWriter = await _principalInserterFactory.CreateAsync(connection);
        await using var userRoleWriter = await _userRoleInserterFactory.CreateAsync(connection);
        await using var accessRoleWriter = await _accessRoleInserterFactory.CreateAsync(connection);
        await using var administratorRoleWriter = await _administratorRoleInserterFactory.CreateAsync(connection);

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
