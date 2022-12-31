﻿namespace PoundPupLegacy.Db;

public class SecondLevelGlobalRegionCreator : IEntityCreator<SecondLevelGlobalRegion>
{
    public static void Create(IEnumerable<SecondLevelGlobalRegion> nodes, NpgsqlConnection connection)
    {
        using var nodeWriter = NodeWriter.Create(connection);
        using var documentableWriter = DocumentableWriter.Create(connection);
        using var termWriter = TermWriter.Create(connection);
        using var geographicalEntityWriter = GeographicalEnityWriter.Create(connection);
        using var globalRegionWriter = GlobalRegionWriter.Create(connection);
        using var secondLevelGlobalRegionWriter = SecondLevelGlobalRegionWriter.Create(connection);
        using var termHierarchyWriter = TermHierarchyWriter.Create(connection);


        foreach (var node in nodes)
        {
            nodeWriter.Write(node);
            documentableWriter.Write(node);
            termWriter.Write(node);
            geographicalEntityWriter.Write(node);
            globalRegionWriter.Write(node);
            secondLevelGlobalRegionWriter.Write(node);

            termHierarchyWriter.Write(new TermHierarchy
            {
                ParentId = node.FirstLevelGlobalRegionId,
                ChildId = node.Id
            });
        }
    }
}
