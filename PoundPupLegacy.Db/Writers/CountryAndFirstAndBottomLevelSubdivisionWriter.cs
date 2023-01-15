namespace PoundPupLegacy.Db.Writers;

internal sealed class CountryAndFirstAndBottomLevelSubdivisionWriter : IDatabaseWriter<CountryAndFirstAndBottomLevelSubdivision>
{
    public static async Task<DatabaseWriter<CountryAndFirstAndBottomLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<CountryAndFirstAndBottomLevelSubdivision>("country_and_first_and_bottom_level_subdivision", connection);
    }
}
