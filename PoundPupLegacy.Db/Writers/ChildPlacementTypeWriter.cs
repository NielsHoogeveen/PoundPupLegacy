namespace PoundPupLegacy.Db.Writers;

internal sealed class ChildPlacementTypeWriter : IDatabaseWriter<ChildPlacementType>
{
    public static async Task<DatabaseWriter<ChildPlacementType>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<ChildPlacementType>("child_placement_type", connection);
    }
}
