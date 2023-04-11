namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DeportationCaseCreator : EntityCreator<DeportationCase>
{
    private readonly IDatabaseInserterFactory<DeportationCase> _deportationCaseInserterFactory;
    private readonly IDatabaseInserterFactory<Case> _caseInserterFactory;
    private readonly IDatabaseInserterFactory<Nameable> _nameableInserterFactory;
    private readonly IDatabaseInserterFactory<Locatable> _locatableInserterFactory;
    private readonly IDatabaseInserterFactory<Documentable> _documentableInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Term> _termInserterFactory;
    private readonly IDatabaseReaderFactory<TermReaderByName> _termReaderFactory;
    private readonly IDatabaseInserterFactory<TermHierarchy> _termHierarchyInserterFactory;
    private readonly IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> _vocabularyIdReaderFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;

    public DeportationCaseCreator(IDatabaseInserterFactory<DeportationCase> deportationCaseInserterFactory,
        IDatabaseInserterFactory<Case> caseInserterFactory,
        IDatabaseInserterFactory<Nameable> nameableInserterFactory,
        IDatabaseInserterFactory<Locatable> locatableInserterFactory,
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
        _deportationCaseInserterFactory = deportationCaseInserterFactory;
        _caseInserterFactory = caseInserterFactory;
        _nameableInserterFactory = nameableInserterFactory;
        _locatableInserterFactory = locatableInserterFactory;
        _documentableInserterFactory = documentableInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _nodeInserterFactory = nodeInserterFactory;
        _termInserterFactory = termInserterFactory;
        _termReaderFactory = termReaderFactory;
        _termHierarchyInserterFactory = termHierarchyInserterFactory;
        _vocabularyIdReaderFactory = vocabularyIdReaderFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }


    public override async Task CreateAsync(IAsyncEnumerable<DeportationCase> deportationCases, IDbConnection connection)
    {

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await _documentableInserterFactory.CreateAsync(connection);
        await using var locatableWriter = await _locatableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await _nameableInserterFactory.CreateAsync(connection);
        await using var caseWriter = await _caseInserterFactory.CreateAsync(connection);
        await using var deportationCaseWriter = await _deportationCaseInserterFactory.CreateAsync(connection);
        await using var termWriter = await _termInserterFactory.CreateAsync(connection);
        await using var termReader = await _termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await _termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await _vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var deportationCase in deportationCases) {
            await nodeWriter.InsertAsync(deportationCase);
            await searchableWriter.InsertAsync(deportationCase);
            await documentableWriter.InsertAsync(deportationCase);
            await locatableWriter.InsertAsync(deportationCase);
            await nameableWriter.InsertAsync(deportationCase);
            await caseWriter.InsertAsync(deportationCase);
            await deportationCaseWriter.InsertAsync(deportationCase);
            await WriteTerms(deportationCase, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in deportationCase.TenantNodes) {
                tenantNode.NodeId = deportationCase.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
