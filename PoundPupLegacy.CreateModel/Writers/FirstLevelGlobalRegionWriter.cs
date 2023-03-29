namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class FirstLevelGlobalRegionWriter : IDatabaseWriter<FirstLevelGlobalRegion>
{
    public static async Task<DatabaseWriter<FirstLevelGlobalRegion>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<FirstLevelGlobalRegion>("first_level_global_region", connection);
    }
}
