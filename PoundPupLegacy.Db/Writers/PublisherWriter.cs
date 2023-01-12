namespace PoundPupLegacy.Db.Writers;

internal class PublisherWriter : IDatabaseWriter<Publisher>
{
    public static async Task<DatabaseWriter<Publisher>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<Publisher>(await SingleIdWriter.CreateSingleIdCommandAsync("publisher", connection));
    }
}
