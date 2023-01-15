namespace PoundPupLegacy.Db.Writers;

internal sealed class WrongfulRemovalCaseWriter : IDatabaseWriter<WrongfulRemovalCase>
{
    public static async Task<DatabaseWriter<WrongfulRemovalCase>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<WrongfulRemovalCase>("wrongful_removal_case", connection);
    }
}
