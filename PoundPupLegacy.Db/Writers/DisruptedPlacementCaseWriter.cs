namespace PoundPupLegacy.Db.Writers;

internal sealed class DisruptedPlacementCaseWriter : IDatabaseWriter<DisruptedPlacementCase>
{
    public static async Task<DatabaseWriter<DisruptedPlacementCase>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<DisruptedPlacementCase>(await SingleIdWriter.CreateSingleIdCommandAsync("disrupted_placement_case", connection));
    }
}
