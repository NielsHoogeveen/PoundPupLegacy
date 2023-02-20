namespace PoundPupLegacy.Db.Writers;

internal sealed class SenatorWriter : IDatabaseWriter<Senator>
{
    public static async Task<DatabaseWriter<Senator>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<Senator>("senator", connection);
    }
}
