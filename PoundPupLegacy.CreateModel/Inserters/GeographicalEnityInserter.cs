namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class GeographicalEnityInserter : IDatabaseInserter<GeographicalEntity>
{
    public static async Task<DatabaseInserter<GeographicalEntity>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<GeographicalEntity>("geographical_entity", connection);
    }
}
