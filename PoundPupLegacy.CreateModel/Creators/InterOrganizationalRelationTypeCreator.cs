namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterOrganizationalRelationTypeCreator : EntityCreator<InterOrganizationalRelationType>
{
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<Nameable> _nameableInserterFactory;
    private readonly IDatabaseInserterFactory<InterOrganizationalRelationType> _interOrganizationalRelationTypeInserterFactory;
    private readonly IDatabaseInserterFactory<Term> _termInserterFactory;
    private readonly IDatabaseReaderFactory<TermReaderByName> _termReaderFactory;
    private readonly IDatabaseInserterFactory<TermHierarchy> _termHierarchyInserterFactory;
    private readonly IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> _vocabularyIdReaderFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    public InterOrganizationalRelationTypeCreator(IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<Nameable> nameableInserterFactory,
        IDatabaseInserterFactory<InterOrganizationalRelationType> interOrganizationalRelationTypeInserterFactory,
        IDatabaseInserterFactory<Term> termInserterFactory,
        IDatabaseReaderFactory<TermReaderByName> termReaderFactory,
        IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
        IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> vocabularyIdReaderFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
    )
    {
        _nodeInserterFactory = nodeInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _nameableInserterFactory = nameableInserterFactory;
        _interOrganizationalRelationTypeInserterFactory = interOrganizationalRelationTypeInserterFactory;
        _termInserterFactory = termInserterFactory;
        _termReaderFactory = termReaderFactory;
        _termHierarchyInserterFactory = termHierarchyInserterFactory;
        _vocabularyIdReaderFactory = vocabularyIdReaderFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }

    public override async Task CreateAsync(IAsyncEnumerable<InterOrganizationalRelationType> interOrganizationalRelationTypes, IDbConnection connection)
    {

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await _nameableInserterFactory.CreateAsync(connection);
        await using var interOrganizationalRelationTypeWriter = await _interOrganizationalRelationTypeInserterFactory.CreateAsync(connection);
        await using var termWriter = await _termInserterFactory.CreateAsync(connection);
        await using var termReader = await _termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await _termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await _vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var interOrganizationalRelationType in interOrganizationalRelationTypes) {
            await nodeWriter.InsertAsync(interOrganizationalRelationType);
            await searchableWriter.InsertAsync(interOrganizationalRelationType);
            await nameableWriter.InsertAsync(interOrganizationalRelationType);
            await interOrganizationalRelationTypeWriter.InsertAsync(interOrganizationalRelationType);
            await WriteTerms(interOrganizationalRelationType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in interOrganizationalRelationType.TenantNodes) {
                tenantNode.NodeId = interOrganizationalRelationType.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
