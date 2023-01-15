namespace PoundPupLegacy.Db.Writers;

internal sealed class CountryAndIntermediateLevelSubdivisionWriter : IDatabaseWriter<CountryAndIntermediateLevelSubdivision>
{
    public static async Task<DatabaseWriter<CountryAndIntermediateLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<CountryAndIntermediateLevelSubdivision>("country_and_intermediate_level_subdivision", connection);
    }
}
