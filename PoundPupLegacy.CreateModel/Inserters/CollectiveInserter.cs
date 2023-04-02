namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CollectiveInserter : IDatabaseInserter<Collective>
{
    public static async Task<DatabaseInserter<Collective>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<Collective>("collective", connection);
    }
}
