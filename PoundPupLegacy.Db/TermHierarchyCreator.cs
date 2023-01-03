namespace PoundPupLegacy.Db;

public class TermHierarchyCreator : IEntityCreator<TermHierarchy>
{
    public static void Create(IEnumerable<TermHierarchy> termHierarchies, NpgsqlConnection connection)
    {

        using var termHierarchyWriter = TermHierarchyWriter.Create(connection);

        foreach (var termHierarchy in termHierarchies)
        {
            termHierarchyWriter.Write(termHierarchy);
        }
    }
}
