namespace PoundPupLegacy.Db.Writers;

internal sealed class FirstLevelSubdivisionWriter : IDatabaseWriter<FirstLevelSubdivision>
{
    public static async Task<DatabaseWriter<FirstLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<FirstLevelSubdivision>(await SingleIdWriter.CreateSingleIdCommandAsync("first_level_subdivision", connection));
    }
}
