namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class SenateBillWriter : IDatabaseWriter<SenateBill>
{
    public static async Task<DatabaseWriter<SenateBill>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<SenateBill>("senate_bill", connection);
    }
}
