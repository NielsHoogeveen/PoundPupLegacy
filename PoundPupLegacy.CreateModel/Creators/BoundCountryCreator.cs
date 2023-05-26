namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BoundCountryCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSearchable> searchableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableDocumentable> documentableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableNameable> nameableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableGeographicalEntity> geographicalEntityInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiablePoliticalEntity> politicalEntityInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableCountry> countryInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSubdivision> subdivisionInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableISOCodedSubdivision> isoCodedSubdivisionInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableBoundCountry> boundCountryInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory

) : IEntityCreatorFactory<EventuallyIdentifiableBoundCountry>
{
    public async Task<IEntityCreator<EventuallyIdentifiableBoundCountry>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<EventuallyIdentifiableBoundCountry>(
            new () {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await geographicalEntityInserterFactory.CreateAsync(connection),
                await politicalEntityInserterFactory.CreateAsync(connection),
                await countryInserterFactory.CreateAsync(connection),
                await subdivisionInserterFactory.CreateAsync(connection),
                await isoCodedSubdivisionInserterFactory.CreateAsync(connection),
                await boundCountryInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
