namespace PoundPupLegacy.Db.Writers;

internal class IntermediateLevelSubdivisionWriter : IDatabaseWriter<IntermediateLevelSubdivision>
{
    public static async Task<DatabaseWriter<IntermediateLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<IntermediateLevelSubdivision>(await SingleIdWriter.CreateSingleIdCommandAsync("intermediate_level_subdivision", connection));
    }
}
