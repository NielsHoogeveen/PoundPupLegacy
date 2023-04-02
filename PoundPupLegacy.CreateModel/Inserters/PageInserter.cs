namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class PageInserter : IDatabaseInserter<Page>
{
    public static async Task<DatabaseInserter<Page>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<Page>("page", connection);
    }
}
