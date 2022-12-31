namespace PoundPupLegacy.Db.Writers;

internal class DenominationWriter : IDatabaseWriter<Denomination>
{
    public static DatabaseWriter<Denomination> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<Denomination>(SingleIdWriter.CreateSingleIdCommand("denomination", connection));
    }
}
