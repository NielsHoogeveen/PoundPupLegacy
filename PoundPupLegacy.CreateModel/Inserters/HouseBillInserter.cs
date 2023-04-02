namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class HouseBillInserter : IDatabaseInserter<HouseBill>
{
    public static async Task<DatabaseInserter<HouseBill>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<HouseBill>("house_bill", connection);
    }
}
