namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class SenatorInserter : IDatabaseInserter<Senator>
{
    public static async Task<DatabaseInserter<Senator>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<Senator>("senator", connection);
    }
}
