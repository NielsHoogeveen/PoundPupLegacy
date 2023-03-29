namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class ISOCodedFirstLevelSubdivisionInserter : IDatabaseInserter<ISOCodedFirstLevelSubdivision>
{
    public static async Task<DatabaseInserter<ISOCodedFirstLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<ISOCodedFirstLevelSubdivision>("iso_coded_first_level_subdivision", connection);
    }
}
