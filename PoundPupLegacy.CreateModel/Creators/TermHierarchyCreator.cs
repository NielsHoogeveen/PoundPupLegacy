namespace PoundPupLegacy.CreateModel.Creators;

public class TermHierarchyCreator : IEntityCreator<TermHierarchy>
{
    public async Task CreateAsync(IAsyncEnumerable<TermHierarchy> termHierarchies, IDbConnection connection)
    {

        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);

        await foreach (var termHierarchy in termHierarchies) {
            await termHierarchyWriter.InsertAsync(termHierarchy);
        }
    }
}
