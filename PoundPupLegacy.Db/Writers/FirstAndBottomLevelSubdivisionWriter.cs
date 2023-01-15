namespace PoundPupLegacy.Db.Writers;

internal sealed class FirstAndBottomLevelSubdivisionWriter : IDatabaseWriter<FirstAndBottomLevelSubdivision>
{
    public static async Task<DatabaseWriter<FirstAndBottomLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<FirstAndBottomLevelSubdivision>("first_and_bottom_level_subdivision", connection);
    }
}