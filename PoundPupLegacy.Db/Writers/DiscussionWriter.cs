namespace PoundPupLegacy.Db.Writers;

internal sealed class DiscussionWriter : IDatabaseWriter<Discussion>
{
    public static async Task<DatabaseWriter<Discussion>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<Discussion>(await SingleIdWriter.CreateSingleIdCommandAsync("discussion", connection));
    }
}
