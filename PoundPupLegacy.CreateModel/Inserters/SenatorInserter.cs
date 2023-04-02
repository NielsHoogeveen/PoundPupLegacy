namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class SenatorInserter : IDatabaseInserter<Senator>
{
    public static async Task<DatabaseInserter<Senator>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<Senator>("senator", connection);
    }
}
