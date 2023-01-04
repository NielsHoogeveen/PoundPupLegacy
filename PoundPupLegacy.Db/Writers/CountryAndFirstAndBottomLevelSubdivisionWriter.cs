namespace PoundPupLegacy.Db.Writers;

internal class CountryAndFirstAndBottomLevelSubdivisionWriter : IDatabaseWriter<CountryAndFirstAndBottomLevelSubdivision>
{
    public static async Task<DatabaseWriter<CountryAndFirstAndBottomLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<CountryAndFirstAndBottomLevelSubdivision>(await SingleIdWriter.CreateSingleIdCommandAsync("country_and_first_and_bottom_level_subdivision", connection));
    }
}
