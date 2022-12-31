namespace PoundPupLegacy.Db.Writers;

internal class GeographicalEnityWriter : IDatabaseWriter<GeographicalEntity>
{
    public static DatabaseWriter<GeographicalEntity> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<GeographicalEntity>(SingleIdWriter.CreateSingleIdCommand("geographical_entity", connection));
    }
}
