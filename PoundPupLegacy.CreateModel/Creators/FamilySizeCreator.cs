namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class FamilySizeCreatorFactory(
    
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<FamilySize.FamilySizeToCreate> familySizeInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<FamilySize.FamilySizeToCreate>
{
    public async Task<IEntityCreator<FamilySize.FamilySizeToCreate>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<FamilySize.FamilySizeToCreate>(
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
