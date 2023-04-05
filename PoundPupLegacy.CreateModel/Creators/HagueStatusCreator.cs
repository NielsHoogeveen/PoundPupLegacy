namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class HagueStatusCreator : EntityCreator<HagueStatus>
{
    private readonly IDatabaseInserterFactory<HagueStatus> _hagueStatusInserterFactory;
    private readonly IDatabaseInserterFactory<Term> _termInserterFactory;
    private readonly IDatabaseInserterFactory<TermHierarchy> _termHierarchyInserterFactory;
    private readonly IDatabaseReaderFactory<TermReaderByName> _termReaderFactory;
    private readonly IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> _vocabularyIdReaderFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<Nameable> _nameableInserterFactory;
    public HagueStatusCreator(IDatabaseInserterFactory<HagueStatus> hagueStatusInserterFactory,
        IDatabaseInserterFactory<Term> termInserterFactory,
        IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
        IDatabaseReaderFactory<TermReaderByName> termReaderFactory,
        IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> vocabularyIdReaderFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory,
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<Nameable> nameableInserterFactory
    )
    {
        _hagueStatusInserterFactory = hagueStatusInserterFactory;
        _termInserterFactory = termInserterFactory;
        _termHierarchyInserterFactory = termHierarchyInserterFactory;
        _termReaderFactory = termReaderFactory;
        _vocabularyIdReaderFactory = vocabularyIdReaderFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
        _nodeInserterFactory = nodeInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _nameableInserterFactory = nameableInserterFactory;
    }

    public override async Task CreateAsync(IAsyncEnumerable<HagueStatus> hagueStatuss, IDbConnection connection)
    {

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await _nameableInserterFactory.CreateAsync(connection);
        await using var hagueStatusWriter = await _hagueStatusInserterFactory.CreateAsync(connection);
        await using var termWriter = await _termInserterFactory.CreateAsync(connection);
        await using var termReader = await _termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await _termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await _vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var hagueStatus in hagueStatuss) {
            await nodeWriter.InsertAsync(hagueStatus);
            await searchableWriter.InsertAsync(hagueStatus);
            await nameableWriter.InsertAsync(hagueStatus);
            await hagueStatusWriter.InsertAsync(hagueStatus);
            await WriteTerms(hagueStatus, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in hagueStatus.TenantNodes) {
                tenantNode.NodeId = hagueStatus.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
