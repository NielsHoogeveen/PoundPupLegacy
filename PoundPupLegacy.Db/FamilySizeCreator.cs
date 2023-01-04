using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class FamilySizeCreator : IEntityCreator<FamilySize>
{
    public static async Task CreateAsync(IEnumerable<FamilySize> familySizes, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var familySizeWriter = await FamilySizeWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReader.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);

        foreach (var familySize in familySizes)
        {
            await nodeWriter.WriteAsync(familySize);
            await nameableWriter.WriteAsync(familySize);
            await familySizeWriter.WriteAsync(familySize);
            await EntityCreator.WriteTerms(familySize, termWriter, termReader, termHierarchyWriter);
        }
    }
}
