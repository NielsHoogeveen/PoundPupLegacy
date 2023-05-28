namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class UnitedStatesPoliticalPartyAffliationCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableUnitedStatesPoliticalPartyAffliation> unitedStatesPoliticalPartyAffliationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiableUnitedStatesPoliticalPartyAffliation>
{
    public async Task<IEntityCreator<EventuallyIdentifiableUnitedStatesPoliticalPartyAffliation>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<EventuallyIdentifiableUnitedStatesPoliticalPartyAffliation>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await unitedStatesPoliticalPartyAffliationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
