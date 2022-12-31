namespace PoundPupLegacy.Db.Writers;

internal class FirstLevelGlobalRegionWriter : IDatabaseWriter<FirstLevelGlobalRegion>
{
    public static DatabaseWriter<FirstLevelGlobalRegion> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<FirstLevelGlobalRegion>(SingleIdWriter.CreateSingleIdCommand("first_level_global_region", connection));
    }
}
