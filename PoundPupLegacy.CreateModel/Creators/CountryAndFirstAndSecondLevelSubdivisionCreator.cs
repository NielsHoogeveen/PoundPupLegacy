namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CountryAndFirstAndSecondLevelSubdivisionCreatorFactory(
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
    IDatabaseInserterFactory<EventuallyIdentifiableSecondLevelSubdivision> secondLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableCountryAndFirstAndSecondLevelSubdivision> countryAndFirstAndSecondLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableBottomLevelSubdivision> bottomLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableFirstAndSecondLevelSubdivision> firstAndSecondLevelSubdivisionInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiableCountryAndFirstAndSecondLevelSubdivision>
{
    public async Task<IEntityCreator<EventuallyIdentifiableCountryAndFirstAndSecondLevelSubdivision>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<EventuallyIdentifiableCountryAndFirstAndSecondLevelSubdivision>(
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
                await bottomLevelSubdivisionInserterFactory.CreateAsync(connection),
                await secondLevelSubdivisionInserterFactory.CreateAsync(connection),
                await firstAndSecondLevelSubdivisionInserterFactory.CreateAsync(connection),
                await countryAndFirstAndSecondLevelSubdivisionInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
