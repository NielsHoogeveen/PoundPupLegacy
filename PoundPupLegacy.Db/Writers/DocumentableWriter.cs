namespace PoundPupLegacy.Db.Writers;

internal sealed class DocumentableWriter : IDatabaseWriter<Documentable>
{
    public static async Task<DatabaseWriter<Documentable>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<Documentable>(await SingleIdWriter.CreateSingleIdCommandAsync("documentable", connection));
    }
}
