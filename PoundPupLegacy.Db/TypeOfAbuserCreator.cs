using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class TypeOfAbuserCreator : IEntityCreator<TypeOfAbuser>
{
    public static void Create(IEnumerable<TypeOfAbuser> typesOfAbuser, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var nameableWriter = NameableWriter.Create(connection);
        using var typeOfAbuserWriter = TypeOfAbuserWriter.Create(connection);
        using var termWriter = TermWriter.Create(connection);
        using var termReader = TermReader.Create(connection);
        using var termHierarchyWriter = TermHierarchyWriter.Create(connection);

        foreach (var typeOfAbuser in typesOfAbuser)
        {
            nodeWriter.Write(typeOfAbuser);
            nameableWriter.Write(typeOfAbuser);
            typeOfAbuserWriter.Write(typeOfAbuser);
            EntityCreator.WriteTerms(typeOfAbuser, termWriter, termReader, termHierarchyWriter);
        }
    }
}
