namespace PoundPupLegacy.Db.Writers;

internal class PartyWriter : IDatabaseWriter<Party>
{
    public static async Task<DatabaseWriter<Party>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<Party>(await SingleIdWriter.CreateSingleIdCommandAsync("party", connection));
    }
}
