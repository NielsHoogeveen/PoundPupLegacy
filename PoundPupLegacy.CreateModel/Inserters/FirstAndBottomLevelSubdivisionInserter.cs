namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class FirstAndBottomLevelSubdivisionInserter : IDatabaseInserter<FirstAndBottomLevelSubdivision>
{
    public static async Task<DatabaseInserter<FirstAndBottomLevelSubdivision>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<FirstAndBottomLevelSubdivision>("first_and_bottom_level_subdivision", connection);
    }
}