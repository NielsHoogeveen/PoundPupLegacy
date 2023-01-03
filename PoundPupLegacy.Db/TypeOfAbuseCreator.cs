using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class TypeOfAbuseCreator : IEntityCreator<TypeOfAbuse>
{
    public static void Create(IEnumerable<TypeOfAbuse> typesOfAbuse, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var nameableWriter = NameableWriter.Create(connection);
        using var typeOfAbuseWriter = TypeOfAbuseWriter.Create(connection);
        using var termWriter = TermWriter.Create(connection);
        using var termReader = TermReader.Create(connection);
        using var termHierarchyWriter = TermHierarchyWriter.Create(connection);

        foreach (var typeOfAbuse in typesOfAbuse)
        {
            nodeWriter.Write(typeOfAbuse);
            nameableWriter.Write(typeOfAbuse);
            typeOfAbuseWriter.Write(typeOfAbuse);
            EntityCreator.WriteTerms(typeOfAbuse, termWriter, termReader, termHierarchyWriter);
        }
    }
}
