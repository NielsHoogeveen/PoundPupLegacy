namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class SenateBillInserter : IDatabaseInserter<SenateBill>
{
    public static async Task<DatabaseInserter<SenateBill>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<SenateBill>("senate_bill", connection);
    }
}
