namespace PoundPupLegacy.Db.Writers;

internal sealed class WrongfulRemovalCaseWriter : IDatabaseWriter<WrongfulRemovalCase>
{
    public static async Task<DatabaseWriter<WrongfulRemovalCase>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<WrongfulRemovalCase>(await SingleIdWriter.CreateSingleIdCommandAsync("wrongful_removal_case", connection));
    }
}
