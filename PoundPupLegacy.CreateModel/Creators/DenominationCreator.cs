namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DenominationCreator : EntityCreator<Denomination>
{
    private readonly IDatabaseInserterFactory<Denomination> _denominationInserterFactory;
    private readonly IDatabaseInserterFactory<Nameable> _nameableInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Term> _termInserterFactory;
    private readonly IDatabaseInserterFactory<TermHierarchy> _termHierarchyInserterFactory;
    private readonly IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> _termReaderFactory;
    private readonly IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> _vocabularyIdReaderFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    public DenominationCreator(
        IDatabaseInserterFactory<Denomination> denominationInserterFactory,
        IDatabaseInserterFactory<Nameable> nameableInserterFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Term> termInserterFactory,
        IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
        IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
        IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
    )
    {
        _denominationInserterFactory = denominationInserterFactory;
        _nameableInserterFactory = nameableInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _nodeInserterFactory = nodeInserterFactory;
        _termInserterFactory = termInserterFactory;
        _termHierarchyInserterFactory = termHierarchyInserterFactory;
        _termReaderFactory = termReaderFactory;
        _vocabularyIdReaderFactory = vocabularyIdReaderFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }

    public override async Task CreateAsync(IAsyncEnumerable<Denomination> denominations, IDbConnection connection)
    {

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await _nameableInserterFactory.CreateAsync(connection);
        await using var denominationWriter = await _denominationInserterFactory.CreateAsync(connection);
        await using var termWriter = await _termInserterFactory.CreateAsync(connection);
        await using var termReader = await _termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await _termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await _vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var denomination in denominations) {
            await nodeWriter.InsertAsync(denomination);
            await searchableWriter.InsertAsync(denomination);
            await nameableWriter.InsertAsync(denomination);
            await denominationWriter.InsertAsync(denomination);
            await WriteTerms(denomination, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in denomination.TenantNodes) {
                tenantNode.NodeId = denomination.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
