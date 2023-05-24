namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BasicCountryCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSearchable> searchableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableDocumentable> documentableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableNameable> nameableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableGeographicalEntity> geographicalEntityInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiablePoliticalEntity> politicalEntityInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableCountry> countryInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableTopLevelCountry> topLevelCountryInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableBasicCountry> basicCountryInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    NameableDetailsCreatorFactory nameableDetailsCreatorFactory
) : INameableCreatorFactory<EventuallyIdentifiableBasicCountry>
{
    public async Task<NameableCreator<EventuallyIdentifiableBasicCountry>> CreateAsync(IDbConnection connection) =>
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
                await basicCountryInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
