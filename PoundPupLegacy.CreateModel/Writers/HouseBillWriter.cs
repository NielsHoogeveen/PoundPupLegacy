namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class HouseBillWriter : IDatabaseWriter<HouseBill>
{
    public static async Task<DatabaseWriter<HouseBill>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<HouseBill>("house_bill", connection);
    }
}
