namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class DisruptedPlacementCaseWriter : IDatabaseWriter<DisruptedPlacementCase>
{
    public static async Task<DatabaseWriter<DisruptedPlacementCase>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<DisruptedPlacementCase>("disrupted_placement_case", connection);
    }
}
