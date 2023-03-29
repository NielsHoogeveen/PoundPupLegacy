namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class FirstAndSecondLevelSubdivisionWriter : IDatabaseWriter<FirstAndSecondLevelSubdivision>
{
    public static async Task<DatabaseWriter<FirstAndSecondLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<FirstAndSecondLevelSubdivision>("first_and_second_level_subdivision", connection);
    }
}