namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class FamilySizeCreator : EntityCreator<FamilySize>
{
    private readonly IDatabaseInserterFactory<FamilySize> _familySizeInserterFactory;
    private readonly IDatabaseInserterFactory<Nameable> _nameableInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Term> _termInserterFactory;
    private readonly IDatabaseReaderFactory<TermReaderByName> _termReaderFactory;
    private readonly IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> _vocabularyIdReaderFactory;
    private readonly IDatabaseInserterFactory<TermHierarchy> _termHierarchyInserterFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    public FamilySizeCreator(IDatabaseInserterFactory<FamilySize> familySizeInserterFactory,
        IDatabaseInserterFactory<Nameable> nameableInserterFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Term> termInserterFactory,
        IDatabaseReaderFactory<TermReaderByName> termReaderFactory,
        IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> vocabularyIdReaderFactory,
        IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
    )
    {
        _familySizeInserterFactory = familySizeInserterFactory;
        _nameableInserterFactory = nameableInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _nodeInserterFactory = nodeInserterFactory;
        _termInserterFactory = termInserterFactory;
        _termReaderFactory = termReaderFactory;
        _vocabularyIdReaderFactory = vocabularyIdReaderFactory;
        _termHierarchyInserterFactory = termHierarchyInserterFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }

    public override async Task CreateAsync(IAsyncEnumerable<FamilySize> familySizes, IDbConnection connection)
    {

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await _nameableInserterFactory.CreateAsync(connection);
        await using var familySizeWriter = await _familySizeInserterFactory.CreateAsync(connection);
        await using var termWriter = await _termInserterFactory.CreateAsync(connection);
        await using var termReader = await _termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await _termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await _vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var familySize in familySizes) {
            await nodeWriter.InsertAsync(familySize);
            await searchableWriter.InsertAsync(familySize);
            await nameableWriter.InsertAsync(familySize);
            await familySizeWriter.InsertAsync(familySize);
            await WriteTerms(familySize, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in familySize.TenantNodes) {
                tenantNode.NodeId = familySize.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
