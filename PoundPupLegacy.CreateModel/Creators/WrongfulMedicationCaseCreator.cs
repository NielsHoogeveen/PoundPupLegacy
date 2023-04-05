namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class WrongfulMedicationCaseCreator : EntityCreator<WrongfulMedicationCase>
{
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<Documentable> _documentableInserterFactory;
    private readonly IDatabaseInserterFactory<Locatable> _locatableInserterFactory;
    private readonly IDatabaseInserterFactory<Nameable> _nameableInserterFactory;
    private readonly IDatabaseInserterFactory<Case> _caseInserterFactory;
    private readonly IDatabaseInserterFactory<WrongfulMedicationCase> _wrongfulMedicationCaseInserterFactory;
    private readonly IDatabaseInserterFactory<Term> _termInserterFactory;
    private readonly IDatabaseReaderFactory<TermReaderByName> _termReaderFactory;
    private readonly IDatabaseInserterFactory<TermHierarchy> _termHierarchyInserterFactory;
    private readonly IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> _vocabularyIdReaderFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    public WrongfulMedicationCaseCreator(
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<Documentable> documentableInserterFactory,
        IDatabaseInserterFactory<Locatable> locatableInserterFactory,
        IDatabaseInserterFactory<Nameable> nameableInserterFactory,
        IDatabaseInserterFactory<Case> caseInserterFactory,
        IDatabaseInserterFactory<WrongfulMedicationCase> wrongfulMedicationCaseInserterFactory,
        IDatabaseInserterFactory<Term> termInserterFactory,
        IDatabaseReaderFactory<TermReaderByName> termReaderFactory,
        IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
        IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> vocabularyIdReaderFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
    )
    {
        _nodeInserterFactory = nodeInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _documentableInserterFactory = documentableInserterFactory;
        _locatableInserterFactory = locatableInserterFactory;
        _nameableInserterFactory = nameableInserterFactory;
        _caseInserterFactory = caseInserterFactory;
        _wrongfulMedicationCaseInserterFactory = wrongfulMedicationCaseInserterFactory;
        _termInserterFactory = termInserterFactory;
        _termReaderFactory = termReaderFactory;
        _termHierarchyInserterFactory = termHierarchyInserterFactory;
        _vocabularyIdReaderFactory = vocabularyIdReaderFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }

    public override async Task CreateAsync(IAsyncEnumerable<WrongfulMedicationCase> wrongfulMedicationCases, IDbConnection connection)
    {
        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await _documentableInserterFactory.CreateAsync(connection);
        await using var locatableWriter = await _locatableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await _nameableInserterFactory.CreateAsync(connection);
        await using var caseWriter = await _caseInserterFactory.CreateAsync(connection);
        await using var wrongfulMedicationCaseWriter = await _wrongfulMedicationCaseInserterFactory.CreateAsync(connection);
        await using var termWriter = await _termInserterFactory.CreateAsync(connection);
        await using var termReader = await _termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await _termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await _vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var wrongfulMedicationCase in wrongfulMedicationCases) {
            await nodeWriter.InsertAsync(wrongfulMedicationCase);
            await searchableWriter.InsertAsync(wrongfulMedicationCase);
            await documentableWriter.InsertAsync(wrongfulMedicationCase);
            await locatableWriter.InsertAsync(wrongfulMedicationCase);
            await nameableWriter.InsertAsync(wrongfulMedicationCase);
            await caseWriter.InsertAsync(wrongfulMedicationCase);
            await wrongfulMedicationCaseWriter.InsertAsync(wrongfulMedicationCase);
            await WriteTerms(wrongfulMedicationCase, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in wrongfulMedicationCase.TenantNodes) {
                tenantNode.NodeId = wrongfulMedicationCase.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
