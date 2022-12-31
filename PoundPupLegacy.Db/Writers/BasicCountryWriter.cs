namespace PoundPupLegacy.Db.Writers;

internal class BasicCountryWriter : IDatabaseWriter<BasicCountry>
{
    public static DatabaseWriter<BasicCountry> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<BasicCountry>(SingleIdWriter.CreateSingleIdCommand("basic_country", connection));
    }
}
