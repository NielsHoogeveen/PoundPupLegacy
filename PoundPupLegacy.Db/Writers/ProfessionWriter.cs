namespace PoundPupLegacy.Db.Writers;

internal class ProfessionWriter : IDatabaseWriter<Profession>
{
    public static DatabaseWriter<Profession> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<Profession>(SingleIdWriter.CreateSingleIdCommand("profession", connection));
    }
}
