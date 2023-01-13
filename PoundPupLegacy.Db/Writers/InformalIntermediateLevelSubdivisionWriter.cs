namespace PoundPupLegacy.Db.Writers;

internal sealed class InformalIntermediateLevelSubdivisionWriter : IDatabaseWriter<InformalIntermediateLevelSubdivision>
{
    public static async Task<DatabaseWriter<InformalIntermediateLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<InformalIntermediateLevelSubdivision>(await SingleIdWriter.CreateSingleIdCommandAsync("informal_intermediate_level_subdivision", connection));
    }
}