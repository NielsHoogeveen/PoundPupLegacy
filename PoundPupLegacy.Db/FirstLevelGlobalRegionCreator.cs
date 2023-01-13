using PoundPupLegacy.Db.Readers;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;

public class FirstLevelGlobalRegionCreator : IEntityCreator<FirstLevelGlobalRegion>
{
    public static async Task CreateAsync(IAsyncEnumerable<FirstLevelGlobalRegion> nodes, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var geographicalEntityWriter = await GeographicalEnityWriter.CreateAsync(connection);
        await using var globalRegionWriter = await GlobalRegionWriter.CreateAsync(connection);
        await using var firstLevelGlobalRegionWriter = await FirstLevelGlobalRegionWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReaderByName.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);
        await using var vocabularyIdReader = await VocabularyIdReaderByOwnerAndName.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var node in nodes)
        {
            await nodeWriter.WriteAsync(node);
            await documentableWriter.WriteAsync(node);
            await nameableWriter.WriteAsync(node);
            await geographicalEntityWriter.WriteAsync(node);
            await globalRegionWriter.WriteAsync(node);
            await firstLevelGlobalRegionWriter.WriteAsync(node);
            await EntityCreator.WriteTerms(node, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in node.TenantNodes)
            {
                tenantNode.NodeId = node.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
