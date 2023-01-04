namespace PoundPupLegacy.Db.Writers;

internal class FirstAndSecondLevelSubdivisionWriter : IDatabaseWriter<FirstAndSecondLevelSubdivision>
{
    public static async Task<DatabaseWriter<FirstAndSecondLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<FirstAndSecondLevelSubdivision>(await SingleIdWriter.CreateSingleIdCommandAsync("first_and_second_level_subdivision", connection));
    }
}