namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SecondLevelGlobalRegionCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<GeographicalEntityToCreate> geographicalEntityInserterFactory,
    IDatabaseInserterFactory<GlobalRegionToCreate> globalRegionInserterFactory,
    IDatabaseInserterFactory<SecondLevelGlobalRegion.ToCreate> secondLevelGlobalRegionInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<SecondLevelGlobalRegion.ToCreate>
{
    public async Task<IEntityCreator<SecondLevelGlobalRegion.ToCreate>> CreateAsync(IDbConnection connection) => 
        new NameableCreator<SecondLevelGlobalRegion.ToCreate>(
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
