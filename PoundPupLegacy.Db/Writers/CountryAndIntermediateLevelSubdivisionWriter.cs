namespace PoundPupLegacy.Db.Writers;

internal class CountryAndIntermediateLevelSubdivisionWriter : IDatabaseWriter<CountryAndIntermediateLevelSubdivision>
{
    public static async Task<DatabaseWriter<CountryAndIntermediateLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<CountryAndIntermediateLevelSubdivision>(await SingleIdWriter.CreateSingleIdCommandAsync("country_and_intermediate_level_subdivision", connection));
    }
}
