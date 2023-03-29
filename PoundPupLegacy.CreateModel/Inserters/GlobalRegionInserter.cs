namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class GlobalRegionInserter : IDatabaseInserter<GlobalRegion>
{
    public static async Task<DatabaseInserter<GlobalRegion>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<GlobalRegion>("global_region", connection);
    }
}
