namespace PoundPupLegacy.Db.Writers;

internal sealed class CollectiveWriter : IDatabaseWriter<Collective>
{
    public static async Task<DatabaseWriter<Collective>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<Collective>("collective", connection);
    }
}
