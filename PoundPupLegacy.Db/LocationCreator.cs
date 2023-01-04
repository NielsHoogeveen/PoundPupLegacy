namespace PoundPupLegacy.Db;

public class LocationCreator : IEntityCreator<Location>
{
    public static async Task CreateAsync(IAsyncEnumerable<Location> locations, NpgsqlConnection connection)
    {

        await using var locationWriter = await LocationWriter.CreateAsync(connection);

        await foreach (var location in locations)
        {
            await locationWriter.WriteAsync(location);
        }
    }
}
