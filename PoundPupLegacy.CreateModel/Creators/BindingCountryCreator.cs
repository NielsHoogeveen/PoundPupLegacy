namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BindingCountryCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<GeographicalEntityToCreate> geographicalEntityInserterFactory,
    IDatabaseInserterFactory<PoliticalEntityToCreate> politicalEntityInserterFactory,
    IDatabaseInserterFactory<CountryToCreate> countryInserterFactory,
    IDatabaseInserterFactory<TopLevelCountryToCreate> topLevelCountryInserterFactory,
    IDatabaseInserterFactory<BindingCountry.ToCreate> bindingCountryInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
    
) : IEntityCreatorFactory<BindingCountry.ToCreate>
{
    public async Task<IEntityCreator<BindingCountry.ToCreate>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<BindingCountry.ToCreate>(
            new () {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await geographicalEntityInserterFactory.CreateAsync(connection),
                await politicalEntityInserterFactory.CreateAsync(connection),
                await countryInserterFactory.CreateAsync(connection),
                await topLevelCountryInserterFactory.CreateAsync(connection),
                await bindingCountryInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
