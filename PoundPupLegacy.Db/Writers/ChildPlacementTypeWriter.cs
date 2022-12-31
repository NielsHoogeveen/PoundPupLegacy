namespace PoundPupLegacy.Db.Writers;

internal class ChildPlacementTypeWriter : IDatabaseWriter<ChildPlacementType>
{
    public static DatabaseWriter<ChildPlacementType> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<ChildPlacementType>(SingleIdWriter.CreateSingleIdCommand("child_placement_type", connection));
    }
}
