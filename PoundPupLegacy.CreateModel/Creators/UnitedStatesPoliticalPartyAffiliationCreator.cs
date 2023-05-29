namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class UnitedStatesPoliticalPartyAffliationCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<UnitedStatesPoliticalPartyAffiliation.UnitedStatesPoliticalPartyAffiliationToCreate> unitedStatesPoliticalPartyAffliationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<UnitedStatesPoliticalPartyAffiliation.UnitedStatesPoliticalPartyAffiliationToCreate>
{
    public async Task<IEntityCreator<UnitedStatesPoliticalPartyAffiliation.UnitedStatesPoliticalPartyAffiliationToCreate>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<UnitedStatesPoliticalPartyAffiliation.UnitedStatesPoliticalPartyAffiliationToCreate>(
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
