namespace PoundPupLegacy.Db;

public class ChildPlacementTypeCreator : IEntityCreator<ChildPlacementType>
{
    public static void Create(IEnumerable<ChildPlacementType> childPlacementTypes, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var nameableWriter = NameableWriter.Create(connection);
        using var childPlacementTypeWriter = ChildPlacementTypeWriter.Create(connection);

        foreach (var childPlacementType in childPlacementTypes)
        {
            nodeWriter.Write(childPlacementType);
            nameableWriter.Write(childPlacementType);
            childPlacementTypeWriter.Write(childPlacementType);
        }
    }
}
