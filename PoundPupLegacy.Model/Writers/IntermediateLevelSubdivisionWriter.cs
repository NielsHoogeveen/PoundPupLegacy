namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class IntermediateLevelSubdivisionWriter : IDatabaseWriter<IntermediateLevelSubdivision>
{
    public static async Task<DatabaseWriter<IntermediateLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<IntermediateLevelSubdivision>("intermediate_level_subdivision", connection);
    }
}
