namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class FirstLevelGlobalRegionCreator(
    IDatabaseInserterFactory<FirstLevelGlobalRegion> firstLevelGlobalRegionInserterFactory,
    IDatabaseInserterFactory<GlobalRegion> globalRegionInserterFactory,
    IDatabaseInserterFactory<GeographicalEntity> geographicalEntityInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<Documentable> documentableInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<FirstLevelGlobalRegion>
{
    public override async Task CreateAsync(IAsyncEnumerable<FirstLevelGlobalRegion> nodes, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await documentableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var geographicalEntityWriter = await geographicalEntityInserterFactory.CreateAsync(connection);
        await using var globalRegionWriter = await globalRegionInserterFactory.CreateAsync(connection);
        await using var firstLevelGlobalRegionWriter = await firstLevelGlobalRegionInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var node in nodes) {
            await nodeWriter.InsertAsync(node);
            await searchableWriter.InsertAsync(node);
            await documentableWriter.InsertAsync(node);
            await nameableWriter.InsertAsync(node);
            await geographicalEntityWriter.InsertAsync(node);
            await globalRegionWriter.InsertAsync(node);
            await firstLevelGlobalRegionWriter.InsertAsync(node);
            await WriteTerms(node, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in node.TenantNodes) {
                tenantNode.NodeId = node.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
