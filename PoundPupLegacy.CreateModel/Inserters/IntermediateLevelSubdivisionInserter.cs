namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class IntermediateLevelSubdivisionInserter : IDatabaseInserter<IntermediateLevelSubdivision>
{
    public static async Task<DatabaseInserter<IntermediateLevelSubdivision>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<IntermediateLevelSubdivision>("intermediate_level_subdivision", connection);
    }
}
