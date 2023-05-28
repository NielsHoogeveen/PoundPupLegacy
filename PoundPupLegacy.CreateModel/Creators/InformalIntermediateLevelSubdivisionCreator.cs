namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InformalIntermediateLevelSubdivisionCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<GeographicalEntityToCreate> geographicalEntityInserterFactory,
    IDatabaseInserterFactory<SubdivisionToCreate> subdivisionInserterFactory,
    IDatabaseInserterFactory<FirstLevelSubdivisionToCreate> firstLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<IntermediateLevelSubdivisionToCreate> intermediateLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableInformalIntermediateLevelSubdivision> informalIntermediateLevelSubdivisionInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiableInformalIntermediateLevelSubdivision>
{
    public async Task<IEntityCreator<EventuallyIdentifiableInformalIntermediateLevelSubdivision>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<EventuallyIdentifiableInformalIntermediateLevelSubdivision>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await geographicalEntityInserterFactory.CreateAsync(connection),
                await subdivisionInserterFactory.CreateAsync(connection),
                await firstLevelSubdivisionInserterFactory.CreateAsync(connection),
                await intermediateLevelSubdivisionInserterFactory.CreateAsync(connection),
                await informalIntermediateLevelSubdivisionInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
