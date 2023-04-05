namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CasePartyTypeCreator : EntityCreator<CasePartyType>
{
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<Nameable> _nameableInserterFactory;
    private readonly IDatabaseInserterFactory<CasePartyType> _casePartyTypeInserterFactory;
    private readonly IDatabaseInserterFactory<Term> _termInserterFactory;
    private readonly IDatabaseReaderFactory<TermReaderByName> _termReaderFactory;
    private readonly IDatabaseInserterFactory<TermHierarchy> _termHierarchyInserterFactory;
    private readonly IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> _vocabularyIdReaderFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;

    public CasePartyTypeCreator(
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<Nameable> nameableInserterFactory,
        IDatabaseInserterFactory<CasePartyType> casePartyTypeInserterFactory,
        IDatabaseInserterFactory<Term> termInserterFactory,
        IDatabaseReaderFactory<TermReaderByName> termReaderFactory,
        IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
        IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> vocabularyIdReaderFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
        )
    {
        _nodeInserterFactory = nodeInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _nameableInserterFactory= nameableInserterFactory;
        _casePartyTypeInserterFactory = casePartyTypeInserterFactory;
        _termInserterFactory = termInserterFactory;
        _termReaderFactory = termReaderFactory;
        _termHierarchyInserterFactory = termHierarchyInserterFactory;
        _vocabularyIdReaderFactory = vocabularyIdReaderFactory; 
        _tenantNodeInserterFactory = tenantNodeInserterFactory;

    }
    public override async Task CreateAsync(IAsyncEnumerable<CasePartyType> casePartyTypes, IDbConnection connection)
    {
        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await _nameableInserterFactory.CreateAsync(connection);
        await using var casePartyTypeWriter = await _casePartyTypeInserterFactory.CreateAsync(connection);
        await using var termWriter = await _termInserterFactory.CreateAsync(connection);
        await using var termReader = await _termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await _termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await _vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var casePartyType in casePartyTypes) {
            await nodeWriter.InsertAsync(casePartyType);
            await searchableWriter.InsertAsync(casePartyType);
            await nameableWriter.InsertAsync(casePartyType);
            await casePartyTypeWriter.InsertAsync(casePartyType);
            await WriteTerms(casePartyType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in casePartyType.TenantNodes) {
                tenantNode.NodeId = casePartyType.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
