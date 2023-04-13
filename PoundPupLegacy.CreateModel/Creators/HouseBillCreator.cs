namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class HouseBillCreator : EntityCreator<HouseBill>
{
    private readonly IDatabaseInserterFactory<HouseBill> _houseBillInserterFactory;
    private readonly IDatabaseInserterFactory<Bill> _billInserterFactory;
    private readonly IDatabaseInserterFactory<Documentable> _documentableInserterFactory;
    private readonly IDatabaseInserterFactory<Nameable> _nameableInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Term> _termInserterFactory;
    private readonly IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> _termReaderFactory;
    private readonly IDatabaseInserterFactory<TermHierarchy> _termHierarchyInserterFactory;
    private readonly IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> _vocabularyIdReaderFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    //Add ctor
    public HouseBillCreator(IDatabaseInserterFactory<HouseBill> houseBillInserterFactory,
        IDatabaseInserterFactory<Bill> billInserterFactory,
        IDatabaseInserterFactory<Documentable> documentableInserterFactory,
        IDatabaseInserterFactory<Nameable> nameableInserterFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Term> termInserterFactory,
        IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
        IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
        IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
    )
    {
        _houseBillInserterFactory = houseBillInserterFactory;
        _billInserterFactory = billInserterFactory;
        _documentableInserterFactory = documentableInserterFactory;
        _nameableInserterFactory = nameableInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _nodeInserterFactory = nodeInserterFactory;
        _termInserterFactory = termInserterFactory;
        _termReaderFactory = termReaderFactory;
        _termHierarchyInserterFactory = termHierarchyInserterFactory;
        _vocabularyIdReaderFactory = vocabularyIdReaderFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }


    public override async Task CreateAsync(IAsyncEnumerable<HouseBill> houseBills, IDbConnection connection)
    {

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await _nameableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await _documentableInserterFactory.CreateAsync(connection);
        await using var billWriter = await _billInserterFactory.CreateAsync(connection);
        await using var houseBillWriter = await _houseBillInserterFactory.CreateAsync(connection);
        await using var termWriter = await _termInserterFactory.CreateAsync(connection);
        await using var termReader = await _termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await _termHierarchyInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await _vocabularyIdReaderFactory.CreateAsync(connection);

        await foreach (var houseBill in houseBills) {
            await nodeWriter.InsertAsync(houseBill);
            await searchableWriter.InsertAsync(houseBill);
            await nameableWriter.InsertAsync(houseBill);
            await documentableWriter.InsertAsync(houseBill);
            await billWriter.InsertAsync(houseBill);
            await houseBillWriter.InsertAsync(houseBill);
            await WriteTerms(houseBill, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);

            foreach (var tenantNode in houseBill.TenantNodes) {
                tenantNode.NodeId = houseBill.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
