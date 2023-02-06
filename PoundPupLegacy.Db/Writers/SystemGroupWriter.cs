namespace PoundPupLegacy.Db.Writers;

internal sealed class SystemGroupWriter : IDatabaseWriter<SystemGroup>
{
    public static async Task<DatabaseWriter<SystemGroup>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<SystemGroup>("system_group", connection);
    }
}
