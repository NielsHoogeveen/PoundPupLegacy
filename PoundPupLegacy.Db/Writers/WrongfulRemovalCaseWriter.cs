namespace PoundPupLegacy.Db.Writers;

internal class WrongfulRemovalCaseWriter : IDatabaseWriter<WrongfulRemovalCase>
{
    public static DatabaseWriter<WrongfulRemovalCase> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<WrongfulRemovalCase>(SingleIdWriter.CreateSingleIdCommand("wrongful_removal_case", connection));
    }
}
