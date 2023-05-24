namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SecondLevelGlobalRegionCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSearchable> searchableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableDocumentable> documentableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableNameable> nameableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableGeographicalEntity> geographicalEntityInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableGlobalRegion> globalRegionInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSecondLevelGlobalRegion> secondLevelGlobalRegionInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    NameableDetailsCreatorFactory nameableDetailsCreatorFactory
) : INameableCreatorFactory<EventuallyIdentifiableSecondLevelGlobalRegion>
{
    public async Task<NameableCreator<EventuallyIdentifiableSecondLevelGlobalRegion>> CreateAsync(IDbConnection connection) => 
        new(
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
