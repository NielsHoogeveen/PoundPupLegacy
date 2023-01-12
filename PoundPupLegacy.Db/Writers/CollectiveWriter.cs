namespace PoundPupLegacy.Db.Writers;

internal class CollectiveWriter : IDatabaseWriter<Collective>
{
    public static async Task<DatabaseWriter<Collective>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<Collective>(await SingleIdWriter.CreateSingleIdCommandAsync("collective", connection));
    }
}
