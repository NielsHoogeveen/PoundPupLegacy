namespace PoundPupLegacy.Db.Writers;

internal class BottomLevelSubdivisionWriter : IDatabaseWriter<BottomLevelSubdivision>
{
    public static async Task<DatabaseWriter<BottomLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<BottomLevelSubdivision>(await SingleIdWriter.CreateSingleIdCommandAsync("bottom_level_subdivision", connection));
    }
}
