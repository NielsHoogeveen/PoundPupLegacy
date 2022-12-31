namespace PoundPupLegacy.Db;

public class ProfessionCreator : IEntityCreator<Profession>
{
    public static void Create(IEnumerable<Profession> professions, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var nameableWriter = NameableWriter.Create(connection);
        using var professionWriter = ProfessionWriter.Create(connection);

        foreach (var profession in professions)
        {
            nodeWriter.Write(profession);
            nameableWriter.Write(profession);
            professionWriter.Write(profession);
        }
    }
}
