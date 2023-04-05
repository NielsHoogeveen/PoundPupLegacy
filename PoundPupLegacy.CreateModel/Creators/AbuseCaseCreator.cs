namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class AbuseCaseCreator : EntityCreator<AbuseCase>
{
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<Documentable> _documentableInserterFactory;
    private readonly IDatabaseInserterFactory<Locatable> _locatableInserterFactory;
    private readonly IDatabaseInserterFactory<Nameable> _nameableInserterFactory;
    private readonly IDatabaseInserterFactory<Case> _caseInserterFactory;
    private readonly IDatabaseInserterFactory<AbuseCase> _abuseCaseInserterFactory;
    private readonly IDatabaseInserterFactory<Term> _termInserterFactory;
    private readonly IDatabaseReaderFactory<TermReaderByName> _termReaderFactory;
    private readonly IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> _vocabularyIdReaderFactory;
    private readonly IDatabaseInserterFactory<TermHierarchy> _termHierarchyInserterFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    public AbuseCaseCreator(
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<Documentable> documentableInserterFactory,
        IDatabaseInserterFactory<Locatable> locatableInserterFactory,
        IDatabaseInserterFactory<Nameable> nameableInserterFactory,
        IDatabaseInserterFactory<Case> caseInserterFactory,
        IDatabaseInserterFactory<AbuseCase> abuseCaseInserterFactory,
        IDatabaseInserterFactory<Term> termInserterFactory,
        IDatabaseReaderFactory<TermReaderByName> termReaderFactory,
        IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> vocabularyIdReaderFactory,
        IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
        ) 
    {
        _nodeInserterFactory = nodeInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _documentableInserterFactory = documentableInserterFactory;
        _locatableInserterFactory = locatableInserterFactory;
        _nameableInserterFactory = nameableInserterFactory;
        _caseInserterFactory = caseInserterFactory;
        _abuseCaseInserterFactory = abuseCaseInserterFactory;
        _termInserterFactory = termInserterFactory;
        _termReaderFactory = termReaderFactory;
        _vocabularyIdReaderFactory = vocabularyIdReaderFactory;
        _termHierarchyInserterFactory = termHierarchyInserterFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }

    public override async Task CreateAsync(IAsyncEnumerable<AbuseCase> abuseCases, IDbConnection connection)
    {

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await _documentableInserterFactory.CreateAsync(connection);
        await using var locatableWriter = await _locatableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await _nameableInserterFactory.CreateAsync(connection);
        await using var caseWriter = await _caseInserterFactory.CreateAsync(connection);
        await using var abuseCaseWriter = await _abuseCaseInserterFactory.CreateAsync(connection);
        await using var termWriter = await _termInserterFactory.CreateAsync(connection);
        await using var termReader = await _termReaderFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await _vocabularyIdReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await _termHierarchyInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var abuseCase in abuseCases) {
            await nodeWriter.InsertAsync(abuseCase);
            await searchableWriter.InsertAsync(abuseCase);
            await documentableWriter.InsertAsync(abuseCase);
            await locatableWriter.InsertAsync(abuseCase);
            await nameableWriter.InsertAsync(abuseCase);
            await caseWriter.InsertAsync(abuseCase);
            await abuseCaseWriter.InsertAsync(abuseCase);
            await WriteTerms(abuseCase, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in abuseCase.TenantNodes) {
                tenantNode.NodeId = abuseCase.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
