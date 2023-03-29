namespace PoundPupLegacy.CreateModel.Creators;

public class SecondLevelGlobalRegionCreator : IEntityCreator<SecondLevelGlobalRegion>
{
    public static async Task CreateAsync(IAsyncEnumerable<SecondLevelGlobalRegion> nodes, NpgsqlConnection connection)
    {
        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var geographicalEntityWriter = await GeographicalEnityInserter.CreateAsync(connection);
        await using var globalRegionWriter = await GlobalRegionInserter.CreateAsync(connection);
        await using var secondLevelGlobalRegionWriter = await SecondLevelGlobalRegionInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var node in nodes) {
            await nodeWriter.WriteAsync(node);
            await searchableWriter.WriteAsync(node);
            await documentableWriter.WriteAsync(node);
            await nameableWriter.WriteAsync(node);
            await geographicalEntityWriter.WriteAsync(node);
            await globalRegionWriter.WriteAsync(node);
            await secondLevelGlobalRegionWriter.WriteAsync(node);
            await EntityCreator.WriteTerms(node, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in node.TenantNodes) {
                tenantNode.NodeId = node.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
