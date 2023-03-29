namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class BasicNameableWriter : IDatabaseWriter<BasicNameable>
{
    public static async Task<DatabaseWriter<BasicNameable>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<BasicNameable>("basic_nameable", connection);
    }
}
