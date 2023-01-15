namespace PoundPupLegacy.Db.Writers;

internal sealed class GlobalRegionWriter : IDatabaseWriter<GlobalRegion>
{
    public static async Task<DatabaseWriter<GlobalRegion>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<GlobalRegion>("global_region", connection);
    }
}
