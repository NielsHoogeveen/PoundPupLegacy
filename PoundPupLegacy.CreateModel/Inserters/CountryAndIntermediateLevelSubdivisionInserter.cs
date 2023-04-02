namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CountryAndIntermediateLevelSubdivisionInserter : IDatabaseInserter<CountryAndIntermediateLevelSubdivision>
{
    public static async Task<DatabaseInserter<CountryAndIntermediateLevelSubdivision>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<CountryAndIntermediateLevelSubdivision>("country_and_intermediate_level_subdivision", connection);
    }
}
