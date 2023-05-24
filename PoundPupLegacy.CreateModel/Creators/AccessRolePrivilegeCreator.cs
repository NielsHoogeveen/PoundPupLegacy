namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class AccessRolePrivilegeCreatorFactory(
    IDatabaseInserterFactory<AccessRolePrivilege> accessRolePrivilegeInserterFactory
) : IInsertingEntityCreatorFactory<AccessRolePrivilege>
{
    public async Task<InsertingEntityCreator<AccessRolePrivilege>> CreateAsync(IDbConnection connection) =>
        new (new () {
            await accessRolePrivilegeInserterFactory.CreateAsync(connection),
        });
}
