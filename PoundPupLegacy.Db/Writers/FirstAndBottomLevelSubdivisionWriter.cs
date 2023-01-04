namespace PoundPupLegacy.Db.Writers;

internal class FirstAndBottomLevelSubdivisionWriter : IDatabaseWriter<FirstAndBottomLevelSubdivision>
{
    public static async Task<DatabaseWriter<FirstAndBottomLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<FirstAndBottomLevelSubdivision>(await SingleIdWriter.CreateSingleIdCommandAsync("first_and_bottom_level_subdivision", connection));
    }
}