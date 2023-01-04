namespace PoundPupLegacy.Db.Writers;

internal class BasicNameableWriter : IDatabaseWriter<BasicNameable>
{
    public static async Task<DatabaseWriter<BasicNameable>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<BasicNameable>(await SingleIdWriter.CreateSingleIdCommandAsync("basic_nameable", connection));
    }
}
