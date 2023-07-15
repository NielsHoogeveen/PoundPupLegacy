namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class CollectiveCreatorFactory(
    IDatabaseInserterFactory<PrincipalToCreate> principalInserterFactory,
    IDatabaseInserterFactory<PublisherToCreate> publisherInserterFactory,
    IDatabaseInserterFactory<Collective> collectiveInserterFactory
) : IEntityCreatorFactory<Collective>
{
    public async Task<IEntityCreator<Collective>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<Collective>(new() {
            await principalInserterFactory.CreateAsync(connection),
            await publisherInserterFactory.CreateAsync(connection),
            await collectiveInserterFactory.CreateAsync(connection)
        });
}