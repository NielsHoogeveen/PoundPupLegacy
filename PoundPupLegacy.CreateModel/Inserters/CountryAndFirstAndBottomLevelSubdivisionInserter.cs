namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CountryAndFirstAndBottomLevelSubdivisionInserter : IDatabaseInserter<CountryAndFirstAndBottomLevelSubdivision>
{
    public static async Task<DatabaseInserter<CountryAndFirstAndBottomLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<CountryAndFirstAndBottomLevelSubdivision>("country_and_first_and_bottom_level_subdivision", connection);
    }
}
