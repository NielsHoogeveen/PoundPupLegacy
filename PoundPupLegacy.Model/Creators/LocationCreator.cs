namespace PoundPupLegacy.CreateModel.Creators;

public class LocationCreator : IEntityCreator<Location>
{
    public static async Task CreateAsync(IAsyncEnumerable<Location> locations, NpgsqlConnection connection)
    {

        await using var locationWriter = await LocationWriter.CreateAsync(connection);
        await using var locationLocatableWriter = await LocationLocatableWriter.CreateAsync(connection);

        await foreach (var location in locations) {
            await locationWriter.WriteAsync(location);
            foreach (var locationLocatable in location.Locatables) {
                await locationLocatableWriter.WriteAsync(locationLocatable);
            }
        }
    }
}
