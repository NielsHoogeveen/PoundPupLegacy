namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class BottomLevelSubdivisionWriter : IDatabaseWriter<BottomLevelSubdivision>
{
    public static async Task<DatabaseWriter<BottomLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<BottomLevelSubdivision>("bottom_level_subdivision", connection);
    }
}
