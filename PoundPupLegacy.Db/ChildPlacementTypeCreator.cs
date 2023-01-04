using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class ChildPlacementTypeCreator : IEntityCreator<ChildPlacementType>
{
    public static async Task CreateAsync(IEnumerable<ChildPlacementType> childPlacementTypes, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var childPlacementTypeWriter = await ChildPlacementTypeWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReader.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);


        foreach (var childPlacementType in childPlacementTypes)
        {
            await nodeWriter.WriteAsync(childPlacementType);
            await nameableWriter.WriteAsync(childPlacementType);
            await childPlacementTypeWriter.WriteAsync(childPlacementType);
            await EntityCreator.WriteTerms(childPlacementType, termWriter, termReader, termHierarchyWriter);
        }
    }
}
