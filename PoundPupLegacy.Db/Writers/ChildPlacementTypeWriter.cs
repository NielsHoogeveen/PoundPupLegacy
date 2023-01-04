namespace PoundPupLegacy.Db.Writers;

internal class ChildPlacementTypeWriter : IDatabaseWriter<ChildPlacementType>
{
    public static async Task<DatabaseWriter<ChildPlacementType>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<ChildPlacementType>(await SingleIdWriter.CreateSingleIdCommandAsync("child_placement_type", connection));
    }
}
