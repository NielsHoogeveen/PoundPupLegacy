namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class ChildPlacementTypeInserter : IDatabaseInserter<ChildPlacementType>
{
    public static async Task<DatabaseInserter<ChildPlacementType>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<ChildPlacementType>("child_placement_type", connection);
    }
}
