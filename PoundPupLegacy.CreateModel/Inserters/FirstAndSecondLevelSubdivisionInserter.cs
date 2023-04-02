namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class FirstAndSecondLevelSubdivisionInserter : IDatabaseInserter<FirstAndSecondLevelSubdivision>
{
    public static async Task<DatabaseInserter<FirstAndSecondLevelSubdivision>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<FirstAndSecondLevelSubdivision>("first_and_second_level_subdivision", connection);
    }
}