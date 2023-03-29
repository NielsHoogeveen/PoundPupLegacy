namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class InformalIntermediateLevelSubdivisionWriter : IDatabaseWriter<InformalIntermediateLevelSubdivision>
{
    public static async Task<DatabaseWriter<InformalIntermediateLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<InformalIntermediateLevelSubdivision>("informal_intermediate_level_subdivision", connection);
    }
}