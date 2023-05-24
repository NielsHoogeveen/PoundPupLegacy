namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BasicSecondLevelSubdivisionCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSearchable> searchableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableDocumentable> documentableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableNameable> nameableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableGeographicalEntity> geographicalEntityInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiablePoliticalEntity> politicalEntityInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSubdivision> subdivisionInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableISOCodedSubdivision> isoCodedSubdivisionInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableBottomLevelSubdivision> bottomLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSecondLevelSubdivision> secondLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableBasicSecondLevelSubdivision> basicSecondLevelSubdivisionInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    NameableDetailsCreatorFactory nameableDetailsCreatorFactory
    
) : INameableCreatorFactory<EventuallyIdentifiableBasicSecondLevelSubdivision>
{
    public async Task<NameableCreator<EventuallyIdentifiableBasicSecondLevelSubdivision>> CreateAsync(IDbConnection connection) =>
        new (
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
