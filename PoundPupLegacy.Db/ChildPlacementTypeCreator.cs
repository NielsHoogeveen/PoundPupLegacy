using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class ChildPlacementTypeCreator : IEntityCreator<ChildPlacementType>
{
    public static void Create(IEnumerable<ChildPlacementType> childPlacementTypes, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var nameableWriter = NameableWriter.Create(connection);
        using var childPlacementTypeWriter = ChildPlacementTypeWriter.Create(connection);
        using var termWriter = TermWriter.Create(connection);
        using var termReader = TermReader.Create(connection);
        using var termHierarchyWriter = TermHierarchyWriter.Create(connection);


        foreach (var childPlacementType in childPlacementTypes)
        {
            nodeWriter.Write(childPlacementType);
            nameableWriter.Write(childPlacementType);
            childPlacementTypeWriter.Write(childPlacementType);
            EntityCreator.WriteTerms(childPlacementType, termWriter, termReader, termHierarchyWriter);
        }
    }
}
