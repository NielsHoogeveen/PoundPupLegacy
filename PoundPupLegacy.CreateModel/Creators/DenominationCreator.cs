namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DenominationCreatorFactory(
    
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<Denomination.DenominationToCreate> denominationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<Denomination.DenominationToCreate>
{
    public async Task<IEntityCreator<Denomination.DenominationToCreate>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<Denomination.DenominationToCreate>(
            new ()
            {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await denominationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
