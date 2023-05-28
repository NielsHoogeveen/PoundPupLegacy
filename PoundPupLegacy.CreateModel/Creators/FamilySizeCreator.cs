namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class FamilySizeCreatorFactory(
    
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableFamilySize> familySizeInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiableFamilySize>
{
    public async Task<IEntityCreator<EventuallyIdentifiableFamilySize>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<EventuallyIdentifiableFamilySize>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await familySizeInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
