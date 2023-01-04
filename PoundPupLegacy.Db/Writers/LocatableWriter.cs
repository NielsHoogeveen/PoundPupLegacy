namespace PoundPupLegacy.Db.Writers;

internal class LocatableWriter : IDatabaseWriter<Locatable>
{
    public static async Task<DatabaseWriter<Locatable>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<Locatable>(await SingleIdWriter.CreateSingleIdCommandAsync("locatable", connection));
    }
}
