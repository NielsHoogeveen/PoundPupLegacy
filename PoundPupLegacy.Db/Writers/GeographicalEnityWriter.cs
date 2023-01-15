namespace PoundPupLegacy.Db.Writers;

internal sealed class GeographicalEnityWriter : IDatabaseWriter<GeographicalEntity>
{
    public static async Task<DatabaseWriter<GeographicalEntity>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<GeographicalEntity>("geographical_entity", connection);
    }
}
