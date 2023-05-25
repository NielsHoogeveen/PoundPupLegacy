namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CollectiveCreatorFactory(
    IDatabaseInserterFactory<Principal> principalInserterFactory,
    IDatabaseInserterFactory<Publisher> publisherInserterFactory,
    IDatabaseInserterFactory<Collective> collectiveInserterFactory
) : IEntityCreatorFactory<Collective>
{
    public async Task<IEntityCreator<Collective>> CreateAsync(IDbConnection connection) => 
        new InsertingEntityCreator<Collective>(new () {
            await principalInserterFactory.CreateAsync(connection),
            await publisherInserterFactory.CreateAsync(connection),
            await collectiveInserterFactory.CreateAsync(connection)
        });
}