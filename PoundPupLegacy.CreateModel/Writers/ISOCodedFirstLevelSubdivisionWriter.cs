namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class ISOCodedFirstLevelSubdivisionWriter : IDatabaseWriter<ISOCodedFirstLevelSubdivision>
{
    public static async Task<DatabaseWriter<ISOCodedFirstLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<ISOCodedFirstLevelSubdivision>("iso_coded_first_level_subdivision", connection);
    }
}
