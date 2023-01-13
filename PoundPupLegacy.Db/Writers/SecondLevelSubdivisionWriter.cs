namespace PoundPupLegacy.Db.Writers;

internal sealed class SecondLevelSubdivisionWriter : IDatabaseWriter<SecondLevelSubdivision>
{
    public static async Task<DatabaseWriter<SecondLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<SecondLevelSubdivision>(await SingleIdWriter.CreateSingleIdCommandAsync("second_level_subdivision", connection));
    }
}
