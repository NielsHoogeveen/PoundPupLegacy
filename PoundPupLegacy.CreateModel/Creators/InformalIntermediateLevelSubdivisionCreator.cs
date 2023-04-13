namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InformalIntermediateLevelSubdivisionCreator : EntityCreator<InformalIntermediateLevelSubdivision>
{
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<Documentable> _documentableInserterFactory;
    private readonly IDatabaseInserterFactory<Nameable> _nameableInserterFactory;
    private readonly IDatabaseInserterFactory<GeographicalEntity> _geographicalEntityInserterFactory;
    private readonly IDatabaseInserterFactory<Subdivision> _subdivisionInserterFactory;
    private readonly IDatabaseInserterFactory<FirstLevelSubdivision> _firstLevelSubdivisionInserterFactory;
    private readonly IDatabaseInserterFactory<IntermediateLevelSubdivision> _intermediateLevelSubdivisionInserterFactory;
    private readonly IDatabaseInserterFactory<InformalIntermediateLevelSubdivision> _informalIntermediateLevelSubdivisionInserterFactory;
    private readonly IDatabaseInserterFactory<Term> _termInserterFactory;
    private readonly IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> _termReaderFactory;
    private readonly IDatabaseInserterFactory<TermHierarchy> _termHierarchyInserterFactory;
    private readonly IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> _vocabularyIdReaderFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;

    public InformalIntermediateLevelSubdivisionCreator(
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<Documentable> documentableInserterFactory,
        IDatabaseInserterFactory<Nameable> nameableInserterFactory,
        IDatabaseInserterFactory<GeographicalEntity> geographicalEntityInserterFactory,
        IDatabaseInserterFactory<Subdivision> subdivisionInserterFactory,
        IDatabaseInserterFactory<FirstLevelSubdivision> firstLevelSubdivisionInserterFactory,
        IDatabaseInserterFactory<IntermediateLevelSubdivision> intermediateLevelSubdivisionInserterFactory,
        IDatabaseInserterFactory<InformalIntermediateLevelSubdivision> informalIntermediateLevelSubdivisionInserterFactory,
        IDatabaseInserterFactory<Term> termInserterFactory,
        IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
        IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
        IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
    )
    {
        _nodeInserterFactory = nodeInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _documentableInserterFactory = documentableInserterFactory;
        _nameableInserterFactory = nameableInserterFactory;
        _geographicalEntityInserterFactory = geographicalEntityInserterFactory;
        _subdivisionInserterFactory = subdivisionInserterFactory;
        _firstLevelSubdivisionInserterFactory = firstLevelSubdivisionInserterFactory;
        _intermediateLevelSubdivisionInserterFactory = intermediateLevelSubdivisionInserterFactory;
        _informalIntermediateLevelSubdivisionInserterFactory = informalIntermediateLevelSubdivisionInserterFactory;
        _termInserterFactory = termInserterFactory;
        _termReaderFactory = termReaderFactory;
        _termHierarchyInserterFactory = termHierarchyInserterFactory;
        _vocabularyIdReaderFactory = vocabularyIdReaderFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }

    public override async Task CreateAsync(IAsyncEnumerable<InformalIntermediateLevelSubdivision> subdivisions, IDbConnection connection)
    {
        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await _documentableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await _nameableInserterFactory.CreateAsync(connection);
        await using var geographicalEntityWriter = await _geographicalEntityInserterFactory.CreateAsync(connection);
        await using var subdivisionWriter = await _subdivisionInserterFactory.CreateAsync(connection);
        await using var firstLevelSubdivisionWriter = await _firstLevelSubdivisionInserterFactory.CreateAsync(connection);
        await using var intermediateLevelSubdivisionWriter = await _intermediateLevelSubdivisionInserterFactory.CreateAsync(connection);
        await using var formalIntermediateLevelSubdivisionWriter = await _informalIntermediateLevelSubdivisionInserterFactory.CreateAsync(connection);
        await using var termWriter = await _termInserterFactory.CreateAsync(connection);
        await using var termReader = await _termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await _termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await _vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var subdivision in subdivisions) {
            await nodeWriter.InsertAsync(subdivision);
            await searchableWriter.InsertAsync(subdivision);
            await documentableWriter.InsertAsync(subdivision);
            await nameableWriter.InsertAsync(subdivision);
            await geographicalEntityWriter.InsertAsync(subdivision);
            await subdivisionWriter.InsertAsync(subdivision);
            await firstLevelSubdivisionWriter.InsertAsync(subdivision);
            await intermediateLevelSubdivisionWriter.InsertAsync(subdivision);
            await formalIntermediateLevelSubdivisionWriter.InsertAsync(subdivision);
            await WriteTerms(subdivision, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in subdivision.TenantNodes) {
                tenantNode.NodeId = subdivision.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
