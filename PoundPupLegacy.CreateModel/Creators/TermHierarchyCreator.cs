namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class TermHierarchyCreator : EntityCreator<TermHierarchy>
{
    private readonly IDatabaseInserterFactory<TermHierarchy> _termHierarchyInserterFactory;
    public TermHierarchyCreator(IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory)
    {
        _termHierarchyInserterFactory = termHierarchyInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<TermHierarchy> termHierarchies, IDbConnection connection)
    {

        await using var termHierarchyWriter = await _termHierarchyInserterFactory.CreateAsync(connection);

        await foreach (var termHierarchy in termHierarchies) {
            await termHierarchyWriter.InsertAsync(termHierarchy);
        }
    }
}
