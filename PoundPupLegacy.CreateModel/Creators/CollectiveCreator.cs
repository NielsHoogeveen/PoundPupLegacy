namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CollectiveCreatorFactory(
    IDatabaseInserterFactory<Principal> principalInserterFactory,
    IDatabaseInserterFactory<Publisher> publisherInserterFactory,
    IDatabaseInserterFactory<Collective> collectiveInserterFactory
) : IInsertingEntityCreatorFactory<Collective>
{
    public async Task<InsertingEntityCreator<Collective>> CreateAsync(IDbConnection connection) => 
        new (new () {
            await principalInserterFactory.CreateAsync(connection),
            await publisherInserterFactory.CreateAsync(connection),
            await collectiveInserterFactory.CreateAsync(connection)
        });
}