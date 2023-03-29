namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class LocatableWriter : IDatabaseWriter<Locatable>
{
    public static async Task<DatabaseWriter<Locatable>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<Locatable>("locatable", connection);
    }
}
