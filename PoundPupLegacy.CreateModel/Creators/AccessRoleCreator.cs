namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class AccessRoleCreatorFactory(
    IDatabaseInserterFactory<Principal> principalInserterFactory,
    IDatabaseInserterFactory<UserRole> userRoleInserterFactory,
    IDatabaseInserterFactory<AccessRole> accessRoleInserterFactory
) : IEntityCreatorFactory<AccessRole>
{

    public async Task<IEntityCreator<AccessRole>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<AccessRole>(new () {
            await principalInserterFactory.CreateAsync(connection),
            await userRoleInserterFactory.CreateAsync(connection),
            await accessRoleInserterFactory.CreateAsync(connection)
        });
}
