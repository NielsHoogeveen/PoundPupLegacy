namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class FirstLevelSubdivisionWriter : IDatabaseWriter<FirstLevelSubdivision>
{
    public static async Task<DatabaseWriter<FirstLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<FirstLevelSubdivision>("first_level_subdivision", connection);
    }
}
