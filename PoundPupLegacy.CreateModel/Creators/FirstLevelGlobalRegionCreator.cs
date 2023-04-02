namespace PoundPupLegacy.CreateModel.Creators;

public class FirstLevelGlobalRegionCreator : IEntityCreator<FirstLevelGlobalRegion>
{
    public async Task CreateAsync(IAsyncEnumerable<FirstLevelGlobalRegion> nodes, IDbConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var geographicalEntityWriter = await GeographicalEnityInserter.CreateAsync(connection);
        await using var globalRegionWriter = await GlobalRegionInserter.CreateAsync(connection);
        await using var firstLevelGlobalRegionWriter = await FirstLevelGlobalRegionInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var node in nodes) {
            await nodeWriter.InsertAsync(node);
            await searchableWriter.InsertAsync(node);
            await documentableWriter.InsertAsync(node);
            await nameableWriter.InsertAsync(node);
            await geographicalEntityWriter.InsertAsync(node);
            await globalRegionWriter.InsertAsync(node);
            await firstLevelGlobalRegionWriter.InsertAsync(node);
            await EntityCreator.WriteTerms(node, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in node.TenantNodes) {
                tenantNode.NodeId = node.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
