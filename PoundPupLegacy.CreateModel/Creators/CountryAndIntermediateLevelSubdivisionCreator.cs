namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CountryAndIntermediateLevelSubdivisionCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSearchable> searchableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableDocumentable> documentableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableNameable> nameableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableGeographicalEntity> geographicalEntityInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiablePoliticalEntity> politicalEntityInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableCountry> countryInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableTopLevelCountry> topLevelCountryInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSubdivision> subdivisionInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableISOCodedSubdivision> isoCodedSubdivisionInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableFirstLevelSubdivision> firstLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableISOCodedFirstLevelSubdivision> isoCodedFirstLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableCountryAndFirstLevelSubdivision> countryAndFirstLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableCountryAndIntermediateLevelSubdivision> countryAndIntermediateLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableIntermediateLevelSubdivision> intermediateLevelSubdivisionInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    NameableDetailsCreatorFactory nameableDetailsCreatorFactory
) : INameableCreatorFactory<EventuallyIdentifiableCountryAndIntermediateLevelSubdivision>
{
    public async Task<NameableCreator<EventuallyIdentifiableCountryAndIntermediateLevelSubdivision>> CreateAsync(IDbConnection connection) =>
        new (
            new () {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await geographicalEntityInserterFactory.CreateAsync(connection),
                await politicalEntityInserterFactory.CreateAsync(connection),
                await countryInserterFactory.CreateAsync(connection),
                await topLevelCountryInserterFactory.CreateAsync(connection),
                await subdivisionInserterFactory.CreateAsync(connection),
                await isoCodedSubdivisionInserterFactory.CreateAsync(connection),
                await firstLevelSubdivisionInserterFactory.CreateAsync(connection),
                await isoCodedFirstLevelSubdivisionInserterFactory.CreateAsync(connection),
                await countryAndFirstLevelSubdivisionInserterFactory.CreateAsync(connection),
                await intermediateLevelSubdivisionInserterFactory.CreateAsync(connection),
                await countryAndIntermediateLevelSubdivisionInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
