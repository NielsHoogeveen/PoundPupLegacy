namespace PoundPupLegacy.Db.Writers;

internal class BindingCountryWriter : IDatabaseWriter<BindingCountry>
{
    public static async Task<DatabaseWriter<BindingCountry>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<BindingCountry>(await SingleIdWriter.CreateSingleIdCommandAsync("binding_country", connection));
    }
}
