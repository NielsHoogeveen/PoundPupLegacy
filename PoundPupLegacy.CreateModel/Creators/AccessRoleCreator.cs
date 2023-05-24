namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class AccessRoleCreatorFactory(
    IDatabaseInserterFactory<Principal> principalInserterFactory,
    IDatabaseInserterFactory<UserRole> userRoleInserterFactory,
    IDatabaseInserterFactory<AccessRole> accessRoleInserterFactory
) : IInsertingEntityCreatorFactory<AccessRole>
{

    public async Task<InsertingEntityCreator<AccessRole>> CreateAsync(IDbConnection connection) =>
        new (new () {
            await principalInserterFactory.CreateAsync(connection),
            await userRoleInserterFactory.CreateAsync(connection),
            await accessRoleInserterFactory.CreateAsync(connection)
        });
}
