namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BindingCountryInserter : IDatabaseInserter<BindingCountry>
{
    public static async Task<DatabaseInserter<BindingCountry>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<BindingCountry>("binding_country", connection);
    }
}
