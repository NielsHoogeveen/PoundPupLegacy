namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class InformalIntermediateLevelSubdivisionInserter : IDatabaseInserter<InformalIntermediateLevelSubdivision>
{
    public static async Task<DatabaseInserter<InformalIntermediateLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<InformalIntermediateLevelSubdivision>("informal_intermediate_level_subdivision", connection);
    }
}