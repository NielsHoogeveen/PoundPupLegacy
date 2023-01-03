using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class FamilySizeCreator : IEntityCreator<FamilySize>
{
    public static void Create(IEnumerable<FamilySize> familySizes, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var nameableWriter = NameableWriter.Create(connection);
        using var familySizeWriter = FamilySizeWriter.Create(connection);
        using var termWriter = TermWriter.Create(connection);
        using var termReader = TermReader.Create(connection);
        using var termHierarchyWriter = TermHierarchyWriter.Create(connection);

        foreach (var familySize in familySizes)
        {
            nodeWriter.Write(familySize);
            nameableWriter.Write(familySize);
            familySizeWriter.Write(familySize);
            EntityCreator.WriteTerms(familySize, termWriter, termReader, termHierarchyWriter);
        }
    }
}
