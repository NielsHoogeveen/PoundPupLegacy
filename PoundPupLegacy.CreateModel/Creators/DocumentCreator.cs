namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DocumentCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<SimpleTextNode> simpleTextNodeInserterFactory,
    IDatabaseInserterFactory<Document> documentInserterFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory,
    IEntityCreator<DocumentableDocument> documentableDocumentCreator
) : EntityCreator<Document>
{
    public override async Task CreateAsync(IAsyncEnumerable<Document> documents, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var simpleTextNodeWriter = await simpleTextNodeInserterFactory.CreateAsync(connection);
        await using var documentWriter = await documentInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

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
                await documentableDocumentCreator.CreateAsync(new DocumentableDocument { DocumentableId = documentable, DocumentId = document.Id!.Value }, connection);
            }
        }
    }
}
