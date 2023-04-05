namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SenateBillCreator : EntityCreator<SenateBill>
{
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<Nameable> _nameableInserterFactory;
    private readonly IDatabaseInserterFactory<Documentable> _documentableInserterFactory;
    private readonly IDatabaseInserterFactory<Bill> _billInserterFactory;
    private readonly IDatabaseInserterFactory<SenateBill> _senateBillInserterFactory;
    private readonly IDatabaseInserterFactory<Term> _termInserterFactory;
    private readonly IDatabaseReaderFactory<TermReaderByName> _termReaderFactory;
    private readonly IDatabaseInserterFactory<TermHierarchy> _termHierarchyInserterFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    private readonly IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> _vocabularyIdReaderFactory;
    public SenateBillCreator(
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<Nameable> nameableInserterFactory,
        IDatabaseInserterFactory<Documentable> documentableInserterFactory,
        IDatabaseInserterFactory<Bill> billInserterFactory,
        IDatabaseInserterFactory<SenateBill> senateBillInserterFactory,
        IDatabaseInserterFactory<Term> termInserterFactory,
        IDatabaseReaderFactory<TermReaderByName> termReaderFactory,
        IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory,
        IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> vocabularyIdReaderFactory
    )
    {
        _nodeInserterFactory = nodeInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _nameableInserterFactory = nameableInserterFactory;
        _documentableInserterFactory = documentableInserterFactory;
        _billInserterFactory = billInserterFactory;
        _senateBillInserterFactory = senateBillInserterFactory;
        _termInserterFactory = termInserterFactory;
        _termReaderFactory = termReaderFactory;
        _termHierarchyInserterFactory = termHierarchyInserterFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
        _vocabularyIdReaderFactory = vocabularyIdReaderFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<SenateBill> senateBills, IDbConnection connection)
    {

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await _nameableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await _documentableInserterFactory.CreateAsync(connection);
        await using var billWriter = await _billInserterFactory.CreateAsync(connection);
        await using var senateBillWriter = await _senateBillInserterFactory.CreateAsync(connection);
        await using var termWriter = await _termInserterFactory.CreateAsync(connection);
        await using var termReader = await _termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await _termHierarchyInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await _vocabularyIdReaderFactory.CreateAsync(connection);

        await foreach (var senateBill in senateBills) {
            await nodeWriter.InsertAsync(senateBill);
            await searchableWriter.InsertAsync(senateBill);
            await nameableWriter.InsertAsync(senateBill);
            await documentableWriter.InsertAsync(senateBill);
            await billWriter.InsertAsync(senateBill);
            await senateBillWriter.InsertAsync(senateBill);
            await WriteTerms(senateBill, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);

            foreach (var tenantNode in senateBill.TenantNodes) {
                tenantNode.NodeId = senateBill.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
