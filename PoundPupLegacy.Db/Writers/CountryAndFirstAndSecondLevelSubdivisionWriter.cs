namespace PoundPupLegacy.Db.Writers;

internal class CountryAndFirstAndSecondLevelSubdivisionWriter : IDatabaseWriter<CountryAndFirstAndSecondLevelSubdivision>
{
    public static async Task<DatabaseWriter<CountryAndFirstAndSecondLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<CountryAndFirstAndSecondLevelSubdivision>(await SingleIdWriter.CreateSingleIdCommandAsync("country_and_first_and_second_level_subdivision", connection));
    }
}
