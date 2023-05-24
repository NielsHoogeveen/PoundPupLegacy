namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class FormalIntermediateLevelSubdivisionCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSearchable> searchableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableDocumentable> documentableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableNameable> nameableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableGeographicalEntity> geographicalEntityInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiablePoliticalEntity> politicalEntityInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSubdivision> subdivisionInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableISOCodedSubdivision> isoCodedSubdivisionInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableFirstLevelSubdivision> firstLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableISOCodedFirstLevelSubdivision> isoCodedFirstLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableIntermediateLevelSubdivision> intermediateLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableFormalIntermediateLevelSubdivision> formalIntermediateLevelSubdivisionInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    NameableDetailsCreatorFactory nameableDetailsCreatorFactory
    
) : INameableCreatorFactory<EventuallyIdentifiableFormalIntermediateLevelSubdivision>
{
    public async Task<NameableCreator<EventuallyIdentifiableFormalIntermediateLevelSubdivision>> CreateAsync(IDbConnection connection) =>
        new(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await geographicalEntityInserterFactory.CreateAsync(connection),
                await politicalEntityInserterFactory.CreateAsync(connection),
                await subdivisionInserterFactory.CreateAsync(connection),
                await isoCodedSubdivisionInserterFactory.CreateAsync(connection),
                await firstLevelSubdivisionInserterFactory.CreateAsync(connection),
                await isoCodedFirstLevelSubdivisionInserterFactory.CreateAsync(connection),
                await intermediateLevelSubdivisionInserterFactory.CreateAsync(connection),
                await formalIntermediateLevelSubdivisionInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
