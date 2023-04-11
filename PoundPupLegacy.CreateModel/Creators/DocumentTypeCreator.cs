namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DocumentTypeCreator : EntityCreator<DocumentType>
{

    private readonly IDatabaseInserterFactory<DocumentType> _documentTypeInserterFactory;
    private readonly IDatabaseInserterFactory<Term> _termInserterFactory;
    private readonly IDatabaseInserterFactory<TermHierarchy> _termHierarchyInserterFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    private readonly IDatabaseReaderFactory<TermReaderByName> _termReaderFactory;
    private readonly IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> _vocabularyIdReaderFactory;
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<Nameable> _nameableInserterFactory;

    public DocumentTypeCreator(
        IDatabaseInserterFactory<DocumentType> documentTypeInserterFactory,
        IDatabaseInserterFactory<Term> termInserterFactory,
        IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory,
        IDatabaseReaderFactory<TermReaderByName> termReaderFactory,
        IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> vocabularyIdReaderFactory,
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<Nameable> nameableInserterFactory
    )
    {
        _documentTypeInserterFactory = documentTypeInserterFactory;
        _termInserterFactory = termInserterFactory;
        _termHierarchyInserterFactory = termHierarchyInserterFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
        _termReaderFactory = termReaderFactory;
        _vocabularyIdReaderFactory = vocabularyIdReaderFactory;
        _nodeInserterFactory = nodeInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _nameableInserterFactory = nameableInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<DocumentType> documentTypes, IDbConnection connection)
    {

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await _nameableInserterFactory.CreateAsync(connection);
        await using var documentTypeWriter = await _documentTypeInserterFactory.CreateAsync(connection);
        await using var termWriter = await _termInserterFactory.CreateAsync(connection);
        await using var termReader = await _termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await _termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await _vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var documentType in documentTypes) {
            await nodeWriter.InsertAsync(documentType);
            await searchableWriter.InsertAsync(documentType);
            await nameableWriter.InsertAsync(documentType);
            await documentTypeWriter.InsertAsync(documentType);
            await WriteTerms(documentType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in documentType.TenantNodes) {
                tenantNode.NodeId = documentType.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
