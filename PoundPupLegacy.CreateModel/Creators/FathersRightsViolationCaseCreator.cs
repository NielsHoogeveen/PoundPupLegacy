namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class FathersRightsViolationCaseCreator : EntityCreator<FathersRightsViolationCase>
{
    private readonly IDatabaseInserterFactory<FathersRightsViolationCase> _fathersRightsViolationCaseInserterFactory;
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<Documentable> _documentableInserterFactory;
    private readonly IDatabaseInserterFactory<Locatable> _locatableInserterFactory;
    private readonly IDatabaseInserterFactory<Nameable> _nameableInserterFactory;
    private readonly IDatabaseInserterFactory<Case> _caseInserterFactory;
    private readonly IDatabaseInserterFactory<Term> _termInserterFactory;
    private readonly IDatabaseReaderFactory<TermReaderByName> _termReaderFactory;
    private readonly IDatabaseInserterFactory<TermHierarchy> _termHierarchyInserterFactory;
    private readonly IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> _vocabularyIdReaderFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    public FathersRightsViolationCaseCreator(
        IDatabaseInserterFactory<FathersRightsViolationCase> fathersRightsViolationCaseInserterFactory,
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<Documentable> documentableInserterFactory,
        IDatabaseInserterFactory<Locatable> locatableInserterFactory,
        IDatabaseInserterFactory<Nameable> nameableInserterFactory,
        IDatabaseInserterFactory<Case> caseInserterFactory,
        IDatabaseInserterFactory<Term> termInserterFactory,
        IDatabaseReaderFactory<TermReaderByName> termReaderFactory,
        IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
        IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> vocabularyIdReaderFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
        )
    {
        _fathersRightsViolationCaseInserterFactory = fathersRightsViolationCaseInserterFactory;
        _nodeInserterFactory = nodeInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _documentableInserterFactory = documentableInserterFactory;
        _locatableInserterFactory = locatableInserterFactory;
        _nameableInserterFactory = nameableInserterFactory;
        _caseInserterFactory = caseInserterFactory;
        _termInserterFactory = termInserterFactory;
        _termReaderFactory = termReaderFactory;
        _termHierarchyInserterFactory = termHierarchyInserterFactory;
        _vocabularyIdReaderFactory = vocabularyIdReaderFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<FathersRightsViolationCase> fathersRightsViolationCases, IDbConnection connection)
    {

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await _documentableInserterFactory.CreateAsync(connection);
        await using var locatableWriter = await _locatableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await _nameableInserterFactory.CreateAsync(connection);
        await using var caseWriter = await _caseInserterFactory.CreateAsync(connection);
        await using var fathersRightsViolationCaseWriter = await _fathersRightsViolationCaseInserterFactory.CreateAsync(connection);
        await using var termWriter = await _termInserterFactory.CreateAsync(connection);
        await using var termReader = await _termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await _termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await _vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var fathersRightsViolationCase in fathersRightsViolationCases) {
            await nodeWriter.InsertAsync(fathersRightsViolationCase);
            await searchableWriter.InsertAsync(fathersRightsViolationCase);
            await documentableWriter.InsertAsync(fathersRightsViolationCase);
            await locatableWriter.InsertAsync(fathersRightsViolationCase);
            await nameableWriter.InsertAsync(fathersRightsViolationCase);
            await caseWriter.InsertAsync(fathersRightsViolationCase);
            await fathersRightsViolationCaseWriter.InsertAsync(fathersRightsViolationCase);
            await WriteTerms(fathersRightsViolationCase, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in fathersRightsViolationCase.TenantNodes) {
                tenantNode.NodeId = fathersRightsViolationCase.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
