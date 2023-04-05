namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterCountryRelationTypeCreator : EntityCreator<InterCountryRelationType>
{
    private readonly IDatabaseInserterFactory<InterCountryRelationType> _interCountryRelationTypeInserterFactory;
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<Nameable> _nameableInserterFactory;
    private readonly IDatabaseInserterFactory<Term> _termInserterFactory;
    private readonly IDatabaseInserterFactory<TermHierarchy> _termHierarchyInserterFactory;
    private readonly IDatabaseReaderFactory<TermReaderByName> _termReaderFactory;
    private readonly IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> _vocabularyIdReaderFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    public InterCountryRelationTypeCreator(
        IDatabaseInserterFactory<InterCountryRelationType> interCountryRelationTypeInserterFactory,
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<Nameable> nameableInserterFactory,
        IDatabaseInserterFactory<Term> termInserterFactory,
        IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
        IDatabaseReaderFactory<TermReaderByName> termReaderFactory,
        IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> vocabularyIdReaderFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
    )
    {
        _interCountryRelationTypeInserterFactory = interCountryRelationTypeInserterFactory;
        _nodeInserterFactory = nodeInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _nameableInserterFactory = nameableInserterFactory;
        _termInserterFactory = termInserterFactory;
        _termHierarchyInserterFactory = termHierarchyInserterFactory;
        _termReaderFactory = termReaderFactory;
        _vocabularyIdReaderFactory = vocabularyIdReaderFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }


    public override async Task CreateAsync(IAsyncEnumerable<InterCountryRelationType> interCountryRelationTypes, IDbConnection connection)
    {

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await _nameableInserterFactory.CreateAsync(connection);
        await using var interCountryRelationTypeWriter = await _interCountryRelationTypeInserterFactory.CreateAsync(connection);
        await using var termWriter = await _termInserterFactory.CreateAsync(connection);
        await using var termReader = await _termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await _termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await _vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var interCountryRelationType in interCountryRelationTypes) {
            await nodeWriter.InsertAsync(interCountryRelationType);
            await searchableWriter.InsertAsync(interCountryRelationType);
            await nameableWriter.InsertAsync(interCountryRelationType);
            await interCountryRelationTypeWriter.InsertAsync(interCountryRelationType);
            await WriteTerms(interCountryRelationType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in interCountryRelationType.TenantNodes) {
                tenantNode.NodeId = interCountryRelationType.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
