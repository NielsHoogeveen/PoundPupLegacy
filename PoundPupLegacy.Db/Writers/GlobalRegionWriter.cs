namespace PoundPupLegacy.Db.Writers;

internal sealed class GlobalRegionWriter : IDatabaseWriter<GlobalRegion>
{
    public static async Task<DatabaseWriter<GlobalRegion>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<GlobalRegion>(await SingleIdWriter.CreateSingleIdCommandAsync("global_region", connection));
    }
}
