using Npgsql;
using PoundPupLegacy.Db.Writers;
using PoundPupLegacy.Model;
namespace PoundPupLegacy.Db;

public class LocationCreator : IEntityCreator<Location>
{
    public static void Create(IEnumerable<Location> locations, NpgsqlConnection connection)
    {

        using var locationWriter = LocationWriter.Create(connection);

        foreach (var location in locations)
        {
            locationWriter.Write(location);
        }
    }
}
