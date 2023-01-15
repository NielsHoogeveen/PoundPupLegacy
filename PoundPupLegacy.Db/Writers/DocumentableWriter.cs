namespace PoundPupLegacy.Db.Writers;

internal sealed class DocumentableWriter : IDatabaseWriter<Documentable>
{
    public static async Task<DatabaseWriter<Documentable>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<Documentable>("documentable", connection);
    }
}
