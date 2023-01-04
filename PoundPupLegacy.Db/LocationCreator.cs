namespace PoundPupLegacy.Db;

public class LocationCreator : IEntityCreator<Location>
{
    public static async Task CreateAsync(IEnumerable<Location> locations, NpgsqlConnection connection)
    {

        await using var locationWriter = await LocationWriter.CreateAsync(connection);

        foreach (var location in locations)
        {
            await locationWriter.WriteAsync(location);
        }
    }
}
