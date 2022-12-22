using Npgsql;
using PoundPupLegacy.Db.Writers;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;

public class FirstLevelGlobalRegionCreator : IEntityCreator<FirstLevelGlobalRegion>
{
    public static void Create(IEnumerable<FirstLevelGlobalRegion> nodes, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var geographicalEntityWriter = GeographicalEnityWriter.Create(connection);
        using var globalRegionWriter = GlobalRegionWriter.Create(connection);
        using var firstLevelGlobalRegionWriter = FirstLevelGlobalRegionWriter.Create(connection);

        foreach (var node in nodes)
        {
            nodeWriter.Write(node);
            geographicalEntityWriter.Write(node);
            globalRegionWriter.Write(node);
            firstLevelGlobalRegionWriter.Write(node);
        }
    }
}
