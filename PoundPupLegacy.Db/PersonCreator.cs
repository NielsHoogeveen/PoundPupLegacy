namespace PoundPupLegacy.Db;

public class PersonCreator : IEntityCreator<BasicPerson>
{
    public static void Create(IEnumerable<BasicPerson> persons, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var documentableWriter = DocumentableWriter.Create(connection);
        using var locatableWriter = LocatableWriter.Create(connection);
        using var partyWriter = PartyWriter.Create(connection);
        using var personWriter = PersonWriter.Create(connection);

        foreach (var person in persons)
        {
            nodeWriter.Write(person);
            documentableWriter.Write(person);
            locatableWriter.Write(person);
            partyWriter.Write(person);
            personWriter.Write(person);
        }
    }
}
