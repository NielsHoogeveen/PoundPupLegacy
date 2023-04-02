namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DocumentCreator : IEntityCreator<Document>
{
    public async Task CreateAsync(IAsyncEnumerable<Document> documents, IDbConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var documentWriter = await DocumentInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);
        await using var documentableDocumentWriter = await DocumentableDocumentInserter.CreateAsync(connection);

        await foreach (var document in documents) {
            await nodeWriter.InsertAsync(document);
            await searchableWriter.InsertAsync(document);
            await documentWriter.InsertAsync(document);
            foreach (var tenantNode in document.TenantNodes) {
                tenantNode.NodeId = document.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
            foreach (var documentable in document.Documentables) {
                await documentableDocumentWriter.InsertAsync(new DocumentableDocument { DocumentableId = documentable, DocumentId = document.Id!.Value });
            }
        }
    }
}
