namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DocumentCreator : EntityCreator<Document>
{
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<SimpleTextNode> _simpleTextNodeInserterFactory;
    private readonly IDatabaseInserterFactory<Document> _documentInserterFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    private readonly IEntityCreator<DocumentableDocument> _documentableDocumentCreator;
    public DocumentCreator(
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<SimpleTextNode> simpleTextNodeInserterFactory,
        IDatabaseInserterFactory<Document> documentInserterFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory,
        IEntityCreator<DocumentableDocument> documentableDocumentCreator)
    {
        _nodeInserterFactory = nodeInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _simpleTextNodeInserterFactory = simpleTextNodeInserterFactory;
        _documentInserterFactory = documentInserterFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
        _documentableDocumentCreator = documentableDocumentCreator;
    }
    public override async Task CreateAsync(IAsyncEnumerable<Document> documents, IDbConnection connection)
    {

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var simpleTextNodeWriter = await _simpleTextNodeInserterFactory.CreateAsync(connection);
        await using var documentWriter = await _documentInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var document in documents) {
            await nodeWriter.InsertAsync(document);
            await searchableWriter.InsertAsync(document);
            await simpleTextNodeWriter.InsertAsync(document);
            await documentWriter.InsertAsync(document);
            foreach (var tenantNode in document.TenantNodes) {
                tenantNode.NodeId = document.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
            foreach (var documentable in document.Documentables) {
                await _documentableDocumentCreator.CreateAsync(new DocumentableDocument { DocumentableId = documentable, DocumentId = document.Id!.Value }, connection);
            }
        }
    }
}
