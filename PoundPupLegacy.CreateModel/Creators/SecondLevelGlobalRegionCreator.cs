namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SecondLevelGlobalRegionCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<GeographicalEntityToCreate> geographicalEntityInserterFactory,
    IDatabaseInserterFactory<GlobalRegionToCreate> globalRegionInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSecondLevelGlobalRegion> secondLevelGlobalRegionInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiableSecondLevelGlobalRegion>
{
    public async Task<IEntityCreator<EventuallyIdentifiableSecondLevelGlobalRegion>> CreateAsync(IDbConnection connection) => 
        new NameableCreator<EventuallyIdentifiableSecondLevelGlobalRegion>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await geographicalEntityInserterFactory.CreateAsync(connection),
                await globalRegionInserterFactory.CreateAsync(connection),
                await secondLevelGlobalRegionInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
