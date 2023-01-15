namespace PoundPupLegacy.Db.Writers;

internal sealed class DiscussionWriter : IDatabaseWriter<Discussion>
{
    public static async Task<DatabaseWriter<Discussion>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<Discussion>("discussion", connection);
    }
}
