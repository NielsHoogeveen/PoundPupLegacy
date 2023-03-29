namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BindingCountryInserter : IDatabaseInserter<BindingCountry>
{
    public static async Task<DatabaseInserter<BindingCountry>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<BindingCountry>("binding_country", connection);
    }
}
