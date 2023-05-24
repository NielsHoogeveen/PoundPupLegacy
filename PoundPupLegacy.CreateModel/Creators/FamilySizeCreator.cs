namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class FamilySizeCreatorFactory(
    
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSearchable> searchableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableNameable> nameableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableFamilySize> familySizeInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    NameableDetailsCreatorFactory nameableDetailsCreatorFactory
) : INameableCreatorFactory<EventuallyIdentifiableFamilySize>
{
    public async Task<NameableCreator<EventuallyIdentifiableFamilySize>> CreateAsync(IDbConnection connection) =>
        new(
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
