namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class UnitedStatesPoliticalPartyAffliationCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<UnitedStatesPoliticalPartyAffiliation.ToCreate> unitedStatesPoliticalPartyAffliationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<UnitedStatesPoliticalPartyAffiliation.ToCreate>
{
    public async Task<IEntityCreator<UnitedStatesPoliticalPartyAffiliation.ToCreate>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<UnitedStatesPoliticalPartyAffiliation.ToCreate>(
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
