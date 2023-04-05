namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SubdivisionTypeCreator : EntityCreator<SubdivisionType>
{
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<Nameable> _nameableInserterFactory;
    private readonly IDatabaseInserterFactory<SubdivisionType> _subdivisionTypeInserterFactory;
    private readonly IDatabaseInserterFactory<Term> _termInserterFactory;
    private readonly IDatabaseReaderFactory<TermReaderByName> _termReaderFactory;
    private readonly IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> _vocabularyIdReaderFactory;
    private readonly IDatabaseInserterFactory<TermHierarchy> _termHierarchyInserterFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    public SubdivisionTypeCreator(
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<Nameable> nameableInserterFactory,
        IDatabaseInserterFactory<SubdivisionType> subdivisionTypeInserterFactory,
        IDatabaseInserterFactory<Term> termInserterFactory,
        IDatabaseReaderFactory<TermReaderByName> termReaderFactory,
        IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> vocabularyIdReaderFactory,
        IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
    )
    {
        _nodeInserterFactory = nodeInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _nameableInserterFactory = nameableInserterFactory;
        _subdivisionTypeInserterFactory = subdivisionTypeInserterFactory;
        _termInserterFactory = termInserterFactory;
        _termReaderFactory = termReaderFactory;
        _vocabularyIdReaderFactory = vocabularyIdReaderFactory;
        _termHierarchyInserterFactory = termHierarchyInserterFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<SubdivisionType> subdivisionTypes, IDbConnection connection)
    {
        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await _nameableInserterFactory.CreateAsync(connection);
        await using var subdivisionTypeWriter = await _subdivisionTypeInserterFactory.CreateAsync(connection);
        await using var termWriter = await _termInserterFactory.CreateAsync(connection);
        await using var termReader = await _termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await _termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await _vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var subdivisionType in subdivisionTypes) {
            await nodeWriter.InsertAsync(subdivisionType);
            await searchableWriter.InsertAsync(subdivisionType);
            await nameableWriter.InsertAsync(subdivisionType);
            await subdivisionTypeWriter.InsertAsync(subdivisionType);
            await WriteTerms(subdivisionType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in subdivisionType.TenantNodes) {
                tenantNode.NodeId = subdivisionType.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
