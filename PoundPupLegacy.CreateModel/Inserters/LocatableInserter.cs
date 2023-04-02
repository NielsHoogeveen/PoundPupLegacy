namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class LocatableInserter : IDatabaseInserter<Locatable>
{
    public static async Task<DatabaseInserter<Locatable>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<Locatable>("locatable", connection);
    }
}
