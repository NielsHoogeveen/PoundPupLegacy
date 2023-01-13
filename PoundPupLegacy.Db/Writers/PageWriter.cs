namespace PoundPupLegacy.Db.Writers;

internal sealed class PageWriter : IDatabaseWriter<Page>
{
    public static async Task<DatabaseWriter<Page>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<Page>(await SingleIdWriter.CreateSingleIdCommandAsync("page", connection));
    }
}
