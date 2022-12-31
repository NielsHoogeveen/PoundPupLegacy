namespace PoundPupLegacy.Db.Writers;

internal class DocumentableWriter : IDatabaseWriter<Documentable>
{
    public static DatabaseWriter<Documentable> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<Documentable>(SingleIdWriter.CreateSingleIdCommand("documentable", connection));
    }
}
