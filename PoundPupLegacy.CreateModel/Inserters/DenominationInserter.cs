namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class DenominationInserter : IDatabaseInserter<Denomination>
{
    public static async Task<DatabaseInserter<Denomination>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<Denomination>("denomination", connection);
    }
}
