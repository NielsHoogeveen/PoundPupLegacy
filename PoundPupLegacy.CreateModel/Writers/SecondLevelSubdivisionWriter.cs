namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class SecondLevelSubdivisionWriter : IDatabaseWriter<SecondLevelSubdivision>
{
    public static async Task<DatabaseWriter<SecondLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<SecondLevelSubdivision>("second_level_subdivision", connection);
    }
}
