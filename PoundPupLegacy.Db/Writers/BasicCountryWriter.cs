namespace PoundPupLegacy.Db.Writers;

internal sealed class BasicCountryWriter : IDatabaseWriter<BasicCountry>
{
    public static async Task<DatabaseWriter<BasicCountry>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<BasicCountry>("basic_country", connection);
    }
}
