namespace PoundPupLegacy.Db;

public class LocationLocatableCreator : IEntityCreator<LocationLocatable>
{
    public static async Task CreateAsync(IEnumerable<LocationLocatable> locationLocatables, NpgsqlConnection connection)
    {

        await using var locationLocatableWriter = await LocationLocatableWriter.CreateAsync(connection);

        foreach (var locationLocatable in locationLocatables)
        {
            await locationLocatableWriter.WriteAsync(locationLocatable);
        }
    }
}
