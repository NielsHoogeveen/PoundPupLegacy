namespace PoundPupLegacy.Db.Writers;

internal class TypeOfAbuserWriter : IDatabaseWriter<TypeOfAbuser>
{
    public static DatabaseWriter<TypeOfAbuser> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<TypeOfAbuser>(SingleIdWriter.CreateSingleIdCommand("type_of_abuser", connection));
    }
}
