namespace PoundPupLegacy.Db;

public class TypeOfAbuseCreator : IEntityCreator<TypeOfAbuse>
{
    public static void Create(IEnumerable<TypeOfAbuse> typesOfAbuse, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var nameableWriter = NameableWriter.Create(connection);
        using var typeOfAbuseWriter = TypeOfAbuseWriter.Create(connection);

        foreach (var typeOfAbuse in typesOfAbuse)
        {
            nodeWriter.Write(typeOfAbuse);
            nameableWriter.Write(typeOfAbuse);
            typeOfAbuseWriter.Write(typeOfAbuse);
        }
    }
}
