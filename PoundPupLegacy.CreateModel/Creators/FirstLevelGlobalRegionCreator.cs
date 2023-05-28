namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class FirstLevelGlobalRegionCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<GeographicalEntityToCreate> geographicalEntityInserterFactory,
    IDatabaseInserterFactory<GlobalRegionToCreate> globalRegionInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableFirstLevelGlobalRegion> firstLevelGlobalRegionInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiableFirstLevelGlobalRegion>
{
    public async Task<IEntityCreator<EventuallyIdentifiableFirstLevelGlobalRegion>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<EventuallyIdentifiableFirstLevelGlobalRegion>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await geographicalEntityInserterFactory.CreateAsync(connection),
                await globalRegionInserterFactory.CreateAsync(connection),
                await firstLevelGlobalRegionInserterFactory.CreateAsync(connection)

            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
