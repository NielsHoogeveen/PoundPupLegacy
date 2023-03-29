namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class DisruptedPlacementCaseInserter : IDatabaseInserter<DisruptedPlacementCase>
{
    public static async Task<DatabaseInserter<DisruptedPlacementCase>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<DisruptedPlacementCase>("disrupted_placement_case", connection);
    }
}
