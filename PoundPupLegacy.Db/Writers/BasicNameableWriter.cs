namespace PoundPupLegacy.Db.Writers;

internal class BasicNameableWriter : IDatabaseWriter<BasicNameable>
{
    public static DatabaseWriter<BasicNameable> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<BasicNameable>(SingleIdWriter.CreateSingleIdCommand("basic_nameable", connection));
    }
}
