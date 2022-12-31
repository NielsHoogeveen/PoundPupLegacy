namespace PoundPupLegacy.Db;

public class LocationLocatableCreator : IEntityCreator<LocationLocatable>
{
    public static void Create(IEnumerable<LocationLocatable> locationLocatables, NpgsqlConnection connection)
    {

        using var locationLocatableWriter = LocationLocatableWriter.Create(connection);

        foreach (var locationLocatable in locationLocatables)
        {
            locationLocatableWriter.Write(locationLocatable);
        }
    }
}
