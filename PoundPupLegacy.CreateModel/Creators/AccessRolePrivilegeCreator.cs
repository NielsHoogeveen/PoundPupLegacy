namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class AccessRolePrivilegeCreatorFactory(
    IDatabaseInserterFactory<AccessRolePrivilege> accessRolePrivilegeInserterFactory
) : IEntityCreatorFactory<AccessRolePrivilege>
{
    public async Task<IEntityCreator<AccessRolePrivilege>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<AccessRolePrivilege>(new () {
            await accessRolePrivilegeInserterFactory.CreateAsync(connection),
        });
}
