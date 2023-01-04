using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class FirstLevelGlobalRegionCreator : IEntityCreator<FirstLevelGlobalRegion>
{
    public static async Task CreateAsync(IEnumerable<FirstLevelGlobalRegion> nodes, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var geographicalEntityWriter = await GeographicalEnityWriter.CreateAsync(connection);
        await using var globalRegionWriter = await GlobalRegionWriter.CreateAsync(connection);
        await using var firstLevelGlobalRegionWriter = await FirstLevelGlobalRegionWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReader.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);

        foreach (var node in nodes)
        {
            await nodeWriter.WriteAsync(node);
            await documentableWriter.WriteAsync(node);
            await nameableWriter.WriteAsync(node);
            await geographicalEntityWriter.WriteAsync(node);
            await globalRegionWriter.WriteAsync(node);
            await firstLevelGlobalRegionWriter.WriteAsync(node);
            await EntityCreator.WriteTerms(node, termWriter, termReader, termHierarchyWriter);
        }
    }
}
