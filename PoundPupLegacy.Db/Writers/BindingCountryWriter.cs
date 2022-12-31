namespace PoundPupLegacy.Db.Writers;

internal class BindingCountryWriter : IDatabaseWriter<BindingCountry>
{
    public static DatabaseWriter<BindingCountry> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<BindingCountry>(SingleIdWriter.CreateSingleIdCommand("binding_country", connection));
    }
}
