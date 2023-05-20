namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class TermHierarchyCreator(IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory) : EntityCreator<TermHierarchy>
{
    public override async Task CreateAsync(IAsyncEnumerable<TermHierarchy> termHierarchies, IDbConnection connection)
    {
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);

        await foreach (var termHierarchy in termHierarchies) {
            await termHierarchyWriter.InsertAsync(termHierarchy);
        }
    }
}
