namespace PoundPupLegacy.Db.Writers;

internal sealed class PublisherWriter : IDatabaseWriter<Publisher>
{
    public static async Task<DatabaseWriter<Publisher>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<Publisher>("publisher", connection);
    }
}
