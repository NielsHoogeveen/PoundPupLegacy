namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class TermHierarchyCreatorFactory(
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory
) : IInsertingEntityCreatorFactory<TermHierarchy>
{
    public async Task<InsertingEntityCreator<TermHierarchy>> CreateAsync(IDbConnection connection) => 
        new(new() {
            await termHierarchyInserterFactory.CreateAsync(connection)
        });
}
