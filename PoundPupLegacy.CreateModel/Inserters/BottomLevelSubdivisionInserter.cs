namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BottomLevelSubdivisionInserter : IDatabaseInserter<BottomLevelSubdivision>
{
    public static async Task<DatabaseInserter<BottomLevelSubdivision>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<BottomLevelSubdivision>("bottom_level_subdivision", connection);
    }
}
