namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BasicSecondLevelSubdivisionCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<GeographicalEntityToCreate> geographicalEntityInserterFactory,
    IDatabaseInserterFactory<PoliticalEntityToCreate> politicalEntityInserterFactory,
    IDatabaseInserterFactory<SubdivisionToCreate> subdivisionInserterFactory,
    IDatabaseInserterFactory<ISOCodedSubdivisionToCreate> isoCodedSubdivisionInserterFactory,
    IDatabaseInserterFactory<BottomLevelSubdivisionToCreate> bottomLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<SecondLevelSubdivisionToCreate> secondLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableBasicSecondLevelSubdivision> basicSecondLevelSubdivisionInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
    
) : IEntityCreatorFactory<EventuallyIdentifiableBasicSecondLevelSubdivision>
{
    public async Task<IEntityCreator<EventuallyIdentifiableBasicSecondLevelSubdivision>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<EventuallyIdentifiableBasicSecondLevelSubdivision>(
            new () {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await geographicalEntityInserterFactory.CreateAsync(connection),
                await politicalEntityInserterFactory.CreateAsync(connection),
                await subdivisionInserterFactory.CreateAsync(connection),
                await isoCodedSubdivisionInserterFactory.CreateAsync(connection),
                await bottomLevelSubdivisionInserterFactory.CreateAsync(connection),
                await secondLevelSubdivisionInserterFactory.CreateAsync(connection),
                await basicSecondLevelSubdivisionInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
