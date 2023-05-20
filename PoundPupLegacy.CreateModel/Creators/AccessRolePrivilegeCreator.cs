namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class AccessRolePrivilegeCreator(
    IDatabaseInserterFactory<AccessRolePrivilege> accessRolePrivilegeInserterFactory
) : EntityCreator<AccessRolePrivilege>
{
    public override async Task CreateAsync(IAsyncEnumerable<AccessRolePrivilege> accessRolePrivileges, IDbConnection connection)
    {
        await using var accessRolePrivilegeWriter = await accessRolePrivilegeInserterFactory.CreateAsync(connection);

        await foreach (var accessRolePrivilege in accessRolePrivileges) {
            await accessRolePrivilegeWriter.InsertAsync(accessRolePrivilege);
        }
    }
}
