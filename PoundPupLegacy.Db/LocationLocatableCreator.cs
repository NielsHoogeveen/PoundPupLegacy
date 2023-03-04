namespace PoundPupLegacy.Db;

public class LocationLocatableCreator : IEntityCreator<LocationLocatable>
{
    public static async Task CreateAsync(IAsyncEnumerable<LocationLocatable> locationLocatables, NpgsqlConnection connection)
    {

        await using var locationLocatableWriter = await LocationLocatableWriter.CreateAsync(connection);

        await foreach (var locationLocatable in locationLocatables) {
            await locationLocatableWriter.WriteAsync(locationLocatable);
        }
    }
}
