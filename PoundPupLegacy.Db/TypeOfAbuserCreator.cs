namespace PoundPupLegacy.Db;

public class TypeOfAbuserCreator : IEntityCreator<TypeOfAbuser>
{
    public static void Create(IEnumerable<TypeOfAbuser> typesOfAbuser, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var nameableWriter = NameableWriter.Create(connection);
        using var typeOfAbuserWriter = TypeOfAbuserWriter.Create(connection);

        foreach (var typeOfAbuser in typesOfAbuser)
        {
            nodeWriter.Write(typeOfAbuser);
            nameableWriter.Write(typeOfAbuser);
            typeOfAbuserWriter.Write(typeOfAbuser);
        }
    }
}
