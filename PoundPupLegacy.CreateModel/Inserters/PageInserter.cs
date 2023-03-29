namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class PageInserter : IDatabaseInserter<Page>
{
    public static async Task<DatabaseInserter<Page>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<Page>("page", connection);
    }
}
