namespace PoundPupLegacy.Db.Writers;

internal class TypeOfAbuseWriter : IDatabaseWriter<TypeOfAbuse>
{
    public static DatabaseWriter<TypeOfAbuse> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<TypeOfAbuse>(SingleIdWriter.CreateSingleIdCommand("party", connection));
    }
}
