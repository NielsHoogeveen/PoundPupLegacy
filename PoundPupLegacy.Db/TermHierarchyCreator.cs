namespace PoundPupLegacy.Db;

public class TermHierarchyCreator : IEntityCreator<TermHierarchy>
{
    public static async Task CreateAsync(IAsyncEnumerable<TermHierarchy> termHierarchies, NpgsqlConnection connection)
    {

        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);

        await foreach (var termHierarchy in termHierarchies)
        {
            await termHierarchyWriter.WriteAsync(termHierarchy);
        }
    }
}
