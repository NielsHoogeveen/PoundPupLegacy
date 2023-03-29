namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class FormalIntermediateLevelSubdivisionWriter : IDatabaseWriter<FormalIntermediateLevelSubdivision>
{
    public static async Task<DatabaseWriter<FormalIntermediateLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<FormalIntermediateLevelSubdivision>("formal_intermediate_level_subdivision", connection);
    }
}