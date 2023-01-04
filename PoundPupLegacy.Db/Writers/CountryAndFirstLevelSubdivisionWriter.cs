namespace PoundPupLegacy.Db.Writers;

internal class CountryAndFirstLevelSubdivisionWriter : IDatabaseWriter<CountryAndFirstLevelSubdivision>
{
    public static async Task<DatabaseWriter<CountryAndFirstLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<CountryAndFirstLevelSubdivision>(await SingleIdWriter.CreateSingleIdCommandAsync("country_and_first_level_subdivision", connection));
    }
}
