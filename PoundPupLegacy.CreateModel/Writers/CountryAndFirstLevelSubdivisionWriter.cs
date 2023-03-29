namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class CountryAndFirstLevelSubdivisionWriter : IDatabaseWriter<CountryAndFirstLevelSubdivision>
{
    public static async Task<DatabaseWriter<CountryAndFirstLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<CountryAndFirstLevelSubdivision>("country_and_first_level_subdivision", connection);
    }
}
