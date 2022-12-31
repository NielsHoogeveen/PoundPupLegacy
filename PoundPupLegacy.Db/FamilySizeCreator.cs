namespace PoundPupLegacy.Db;

public class FamilySizeCreator : IEntityCreator<FamilySize>
{
    public static void Create(IEnumerable<FamilySize> familySizes, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var nameableWriter = NameableWriter.Create(connection);
        using var familySizeWriter = FamilySizeWriter.Create(connection);

        foreach (var familySize in familySizes)
        {
            nodeWriter.Write(familySize);
            nameableWriter.Write(familySize);
            familySizeWriter.Write(familySize);
        }
    }
}
