namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class CollectiveUserCreatorFactory(
    IDatabaseInserterFactory<CollectiveUser> collectiveUserInserterFactory
) : IEntityCreatorFactory<CollectiveUser>
{
    public async Task<IEntityCreator<CollectiveUser>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<CollectiveUser>(new() {
            await collectiveUserInserterFactory.CreateAsync(connection)
        });
}
