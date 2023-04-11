namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ActCreator : EntityCreator<Act>
{
    private readonly IDatabaseInserterFactory<Act> _actInserterFactory;
    private readonly IDatabaseInserterFactory<Term> _termInserterFactory;
    private readonly IDatabaseInserterFactory<TermHierarchy> _termHierarchyInserterFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    private readonly IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> _vocabularyIdReaderFactory;
    private readonly IDatabaseReaderFactory<TermReaderByName> _termReaderFactory;
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<Documentable> _documentableInserterFactory;
    private readonly IDatabaseInserterFactory<Nameable> _nameableInserterFactory;

    public ActCreator(
        IDatabaseInserterFactory<Act> actInserterFactory,
        IDatabaseInserterFactory<Term> termInserterFactory,
        IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory,
        IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> vocabularyIdReaderFactory,
        IDatabaseReaderFactory<TermReaderByName> termReaderFactory,
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<Documentable> documentableInserterFactory,
        IDatabaseInserterFactory<Nameable> nameableInserterFactory
        )
    {
        _actInserterFactory = actInserterFactory;
        _nodeInserterFactory = nodeInserterFactory;
        _documentableInserterFactory = documentableInserterFactory;
        _nameableInserterFactory = nameableInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
        _termHierarchyInserterFactory = termHierarchyInserterFactory;
        _termInserterFactory = termInserterFactory;
        _vocabularyIdReaderFactory = vocabularyIdReaderFactory;
        _termReaderFactory = termReaderFactory;

    }
    public override async Task CreateAsync(IAsyncEnumerable<Act> acts, IDbConnection connection)
    {

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await _nameableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await _documentableInserterFactory.CreateAsync(connection);
        await using var actWriter = await _actInserterFactory.CreateAsync(connection);
        await using var termWriter = await _termInserterFactory.CreateAsync(connection);
        await using var termReader = await _termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await _termHierarchyInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await _vocabularyIdReaderFactory.CreateAsync(connection);

        await foreach (var act in acts) {
            await nodeWriter.InsertAsync(act);
            await searchableWriter.InsertAsync(act);
            await nameableWriter.InsertAsync(act);
            await documentableWriter.InsertAsync(act);
            await actWriter.InsertAsync(act);
            await WriteTerms(act, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);

            foreach (var tenantNode in act.TenantNodes) {
                tenantNode.NodeId = act.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
