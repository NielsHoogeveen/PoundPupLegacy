namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CollectiveUserCreatorFactory(
    IDatabaseInserterFactory<CollectiveUser> collectiveUserInserterFactory
) : IInsertingEntityCreatorFactory<CollectiveUser>
{
    public async Task<InsertingEntityCreator<CollectiveUser>> CreateAsync(IDbConnection connection) => 
        new InsertingEntityCreator<CollectiveUser>(new () {
            await collectiveUserInserterFactory.CreateAsync(connection)
        });
}
