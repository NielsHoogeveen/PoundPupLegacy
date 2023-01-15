namespace PoundPupLegacy.Db.Writers;

internal sealed class PartyWriter : IDatabaseWriter<Party>
{
    public static async Task<DatabaseWriter<Party>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<Party>("party", connection);
    }
}
