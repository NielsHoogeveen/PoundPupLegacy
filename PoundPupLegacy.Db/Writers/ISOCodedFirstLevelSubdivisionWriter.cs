namespace PoundPupLegacy.Db.Writers;

internal sealed class ISOCodedFirstLevelSubdivisionWriter : IDatabaseWriter<ISOCodedFirstLevelSubdivision>
{
    public static async Task<DatabaseWriter<ISOCodedFirstLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<ISOCodedFirstLevelSubdivision>(await SingleIdWriter.CreateSingleIdCommandAsync("iso_coded_first_level_subdivision", connection));
    }
}
