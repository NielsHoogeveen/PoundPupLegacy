namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CountryAndFirstAndBottomLevelSubdivisionCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<GeographicalEntityToCreate> geographicalEntityInserterFactory,
    IDatabaseInserterFactory<PoliticalEntityToCreate> politicalEntityInserterFactory,
    IDatabaseInserterFactory<CountryToCreate> countryInserterFactory,
    IDatabaseInserterFactory<TopLevelCountryToCreate> topLevelCountryInserterFactory,
    IDatabaseInserterFactory<SubdivisionToCreate> subdivisionInserterFactory,
    IDatabaseInserterFactory<ISOCodedSubdivisionToCreate> isoCodedSubdivisionInserterFactory,
    IDatabaseInserterFactory<FirstLevelSubdivisionToCreate> firstLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<ISOCodedFirstLevelSubdivisionToCreate> isofirstLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<BottomLevelSubdivisionToCreate> bottomLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<CountryAndFirstLevelSubdivisionToCreate> countryAndFirstLevelSubdivisionFactory,
    IDatabaseInserterFactory<CountryAndFirstAndBottomLevelSubdivision.ToCreate> countryAndFirstAndBottomLevelSubdivisionFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<CountryAndFirstAndBottomLevelSubdivision.ToCreate>
{
    public async Task<IEntityCreator<CountryAndFirstAndBottomLevelSubdivision.ToCreate>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<CountryAndFirstAndBottomLevelSubdivision.ToCreate>(
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
                await isofirstLevelSubdivisionInserterFactory.CreateAsync(connection),
                await countryAndFirstLevelSubdivisionFactory.CreateAsync(connection),
                await bottomLevelSubdivisionInserterFactory.CreateAsync(connection),
                await countryAndFirstAndBottomLevelSubdivisionFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
