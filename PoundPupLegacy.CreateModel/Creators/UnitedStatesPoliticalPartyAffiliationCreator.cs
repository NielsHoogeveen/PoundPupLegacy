namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class UnitedStatesPoliticalPartyAffliationCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSearchable> searchableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableNameable> nameableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableUnitedStatesPoliticalPartyAffliation> unitedStatesPoliticalPartyAffliationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    NameableDetailsCreatorFactory nameableDetailsCreatorFactory
) : INameableCreatorFactory<EventuallyIdentifiableUnitedStatesPoliticalPartyAffliation>
{
    public async Task<NameableCreator<EventuallyIdentifiableUnitedStatesPoliticalPartyAffliation>> CreateAsync(IDbConnection connection) =>
        new(
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
