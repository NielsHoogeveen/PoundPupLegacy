namespace PoundPupLegacy.Db.Writers;

internal sealed class FirstLevelGlobalRegionWriter : IDatabaseWriter<FirstLevelGlobalRegion>
{
    public static async Task<DatabaseWriter<FirstLevelGlobalRegion>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<FirstLevelGlobalRegion>(await SingleIdWriter.CreateSingleIdCommandAsync("first_level_global_region", connection));
    }
}
