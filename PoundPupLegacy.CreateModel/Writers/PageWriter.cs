namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class PageWriter : IDatabaseWriter<Page>
{
    public static async Task<DatabaseWriter<Page>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<Page>("page", connection);
    }
}
