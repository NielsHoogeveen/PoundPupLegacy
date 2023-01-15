namespace PoundPupLegacy.Db.Writers;

internal sealed class CountryAndFirstAndSecondLevelSubdivisionWriter : IDatabaseWriter<CountryAndFirstAndSecondLevelSubdivision>
{
    public static async Task<DatabaseWriter<CountryAndFirstAndSecondLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<CountryAndFirstAndSecondLevelSubdivision>("country_and_first_and_second_level_subdivision", connection);
    }
}
