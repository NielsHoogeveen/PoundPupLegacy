namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class FirstLevelGlobalRegionCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<GeographicalEntityToCreate> geographicalEntityInserterFactory,
    IDatabaseInserterFactory<GlobalRegionToCreate> globalRegionInserterFactory,
    IDatabaseInserterFactory<FirstLevelGlobalRegion.ToCreate> firstLevelGlobalRegionInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<FirstLevelGlobalRegion.ToCreate>
{
    public async Task<IEntityCreator<FirstLevelGlobalRegion.ToCreate>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<FirstLevelGlobalRegion.ToCreate>(
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
