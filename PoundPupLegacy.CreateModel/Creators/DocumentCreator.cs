namespace PoundPupLegacy.CreateModel.Creators;

public class DocumentCreator : IEntityCreator<Document>
{
    public static async Task CreateAsync(IAsyncEnumerable<Document> documents, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var documentWriter = await DocumentInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);
        await using var documentableDocumentWriter = await DocumentableDocumentInserter.CreateAsync(connection);

        await foreach (var document in documents) {
            await nodeWriter.WriteAsync(document);
            await searchableWriter.WriteAsync(document);
            await documentWriter.WriteAsync(document);
            foreach (var tenantNode in document.TenantNodes) {
                tenantNode.NodeId = document.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }
            foreach (var documentable in document.Documentables) {
                await documentableDocumentWriter.WriteAsync(new DocumentableDocument { DocumentableId = documentable, DocumentId = document.Id!.Value });
            }
        }
    }
}
