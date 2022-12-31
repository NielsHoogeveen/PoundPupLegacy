namespace PoundPupLegacy.Db;

public class FirstLevelGlobalRegionCreator : IEntityCreator<FirstLevelGlobalRegion>
{
    public static void Create(IEnumerable<FirstLevelGlobalRegion> nodes, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var documentableWriter = DocumentableWriter.Create(connection);
        using var termWriter = TermWriter.Create(connection);
        using var geographicalEntityWriter = GeographicalEnityWriter.Create(connection);
        using var globalRegionWriter = GlobalRegionWriter.Create(connection);
        using var firstLevelGlobalRegionWriter = FirstLevelGlobalRegionWriter.Create(connection);

        foreach (var node in nodes)
        {
            nodeWriter.Write(node);
            documentableWriter.Write(node);
            termWriter.Write(node);
            geographicalEntityWriter.Write(node);
            globalRegionWriter.Write(node);
            firstLevelGlobalRegionWriter.Write(node);
        }
    }
}
