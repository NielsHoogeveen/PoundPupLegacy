namespace PoundPupLegacy.Db.Writers;

internal class FormalIntermediateLevelSubdivisionWriter : IDatabaseWriter<FormalIntermediateLevelSubdivision>
{
    public static async Task<DatabaseWriter<FormalIntermediateLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<FormalIntermediateLevelSubdivision>(await SingleIdWriter.CreateSingleIdCommandAsync("formal_intermediate_level_subdivision", connection));
    }
}