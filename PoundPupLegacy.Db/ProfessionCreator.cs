using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class ProfessionCreator : IEntityCreator<Profession>
{
    public static void Create(IEnumerable<Profession> professions, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var nameableWriter = NameableWriter.Create(connection);
        using var professionWriter = ProfessionWriter.Create(connection);
        using var termWriter = TermWriter.Create(connection);
        using var termReader = TermReader.Create(connection);
        using var termHierarchyWriter = TermHierarchyWriter.Create(connection);

        foreach (var profession in professions)
        {
            nodeWriter.Write(profession);
            nameableWriter.Write(profession);
            professionWriter.Write(profession);
            EntityCreator.WriteTerms(profession, termWriter, termReader, termHierarchyWriter);
        }
    }
}
