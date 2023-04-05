namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class AccessRolePrivilegeCreator : EntityCreator<AccessRolePrivilege>
{
    private readonly IDatabaseInserterFactory<AccessRolePrivilege> _accessRolePrivilegeInserterFactory;
    public AccessRolePrivilegeCreator(
        IDatabaseInserterFactory<AccessRolePrivilege> accessRolePrivilegeInserterFactory
    )
    {
        _accessRolePrivilegeInserterFactory = accessRolePrivilegeInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<AccessRolePrivilege> accessRolePrivileges, IDbConnection connection)
    {

        await using var accessRolePrivilegeWriter = await _accessRolePrivilegeInserterFactory.CreateAsync(connection);

        await foreach (var accessRolePrivilege in accessRolePrivileges) {
            await accessRolePrivilegeWriter.InsertAsync(accessRolePrivilege);
        }

    }
}
