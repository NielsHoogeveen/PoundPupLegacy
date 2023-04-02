namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CountryAndFirstAndSecondLevelSubdivisionInserter : IDatabaseInserter<CountryAndFirstAndSecondLevelSubdivision>
{
    public static async Task<DatabaseInserter<CountryAndFirstAndSecondLevelSubdivision>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<CountryAndFirstAndSecondLevelSubdivision>("country_and_first_and_second_level_subdivision", connection);
    }
}
