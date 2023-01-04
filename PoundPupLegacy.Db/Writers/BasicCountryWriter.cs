namespace PoundPupLegacy.Db.Writers;

internal class BasicCountryWriter : IDatabaseWriter<BasicCountry>
{
    public static async Task<DatabaseWriter<BasicCountry>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<BasicCountry>(await SingleIdWriter.CreateSingleIdCommandAsync("basic_country", connection));
    }
}
