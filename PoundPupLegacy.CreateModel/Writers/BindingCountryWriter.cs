namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class BindingCountryWriter : IDatabaseWriter<BindingCountry>
{
    public static async Task<DatabaseWriter<BindingCountry>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<BindingCountry>("binding_country", connection);
    }
}
