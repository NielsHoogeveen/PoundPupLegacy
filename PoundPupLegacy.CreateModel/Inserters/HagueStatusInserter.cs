namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class HagueStatusInserter : IDatabaseInserter<HagueStatus>
{
    public static async Task<DatabaseInserter<HagueStatus>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<HagueStatus>("hague_status", connection);
    }
}
