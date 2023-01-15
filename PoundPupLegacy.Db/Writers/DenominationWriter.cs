namespace PoundPupLegacy.Db.Writers;

internal sealed class DenominationWriter : IDatabaseWriter<Denomination>
{
    public static async Task<DatabaseWriter<Denomination>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<Denomination>("denomination", connection);
    }
}
