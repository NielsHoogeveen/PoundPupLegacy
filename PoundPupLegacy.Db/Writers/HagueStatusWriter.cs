namespace PoundPupLegacy.Db.Writers;

internal sealed class HagueStatusWriter : IDatabaseWriter<HagueStatus>
{
    public static async Task<DatabaseWriter<HagueStatus>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<HagueStatus>("hague_status", connection);
    }
}
