namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CountryAndFirstLevelSubdivisionInserter : IDatabaseInserter<CountryAndFirstLevelSubdivision>
{
    public static async Task<DatabaseInserter<CountryAndFirstLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<CountryAndFirstLevelSubdivision>("country_and_first_level_subdivision", connection);
    }
}
