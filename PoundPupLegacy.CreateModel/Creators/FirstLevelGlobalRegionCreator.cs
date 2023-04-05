namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class FirstLevelGlobalRegionCreator : EntityCreator<FirstLevelGlobalRegion>
{
    private readonly IDatabaseInserterFactory<FirstLevelGlobalRegion> _firstLevelGlobalRegionInserterFactory;
    private readonly IDatabaseInserterFactory<GlobalRegion> _globalRegionInserterFactory;
    private readonly IDatabaseInserterFactory<GeographicalEntity> _geographicalEntityInserterFactory;
    private readonly IDatabaseInserterFactory<Nameable> _nameableInserterFactory;
    private readonly IDatabaseInserterFactory<Documentable> _documentableInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Term> _termInserterFactory;
    private readonly IDatabaseReaderFactory<TermReaderByName> _termReaderFactory;
    private readonly IDatabaseInserterFactory<TermHierarchy> _termHierarchyInserterFactory;
    private readonly IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> _vocabularyIdReaderFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    //Add constructor
    public FirstLevelGlobalRegionCreator(
        IDatabaseInserterFactory<FirstLevelGlobalRegion> firstLevelGlobalRegionInserterFactory,
        IDatabaseInserterFactory<GlobalRegion> globalRegionInserterFactory,
        IDatabaseInserterFactory<GeographicalEntity> geographicalEntityInserterFactory,
        IDatabaseInserterFactory<Nameable> nameableInserterFactory,
        IDatabaseInserterFactory<Documentable> documentableInserterFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Term> termInserterFactory,
        IDatabaseReaderFactory<TermReaderByName> termReaderFactory,
        IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
        IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> vocabularyIdReaderFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
    )
    {
        _firstLevelGlobalRegionInserterFactory = firstLevelGlobalRegionInserterFactory;
        _globalRegionInserterFactory = globalRegionInserterFactory;
        _geographicalEntityInserterFactory = geographicalEntityInserterFactory;
        _nameableInserterFactory = nameableInserterFactory;
        _documentableInserterFactory = documentableInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _nodeInserterFactory = nodeInserterFactory;
        _termInserterFactory = termInserterFactory;
        _termReaderFactory = termReaderFactory;
        _termHierarchyInserterFactory = termHierarchyInserterFactory;
        _vocabularyIdReaderFactory = vocabularyIdReaderFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }

    public override async Task CreateAsync(IAsyncEnumerable<FirstLevelGlobalRegion> nodes, IDbConnection connection)
    {

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await _documentableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await _nameableInserterFactory.CreateAsync(connection);
        await using var geographicalEntityWriter = await _geographicalEntityInserterFactory.CreateAsync(connection);
        await using var globalRegionWriter = await _globalRegionInserterFactory.CreateAsync(connection);
        await using var firstLevelGlobalRegionWriter = await _firstLevelGlobalRegionInserterFactory.CreateAsync(connection);
        await using var termWriter = await _termInserterFactory.CreateAsync(connection);
        await using var termReader = await _termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await _termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await _vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

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
