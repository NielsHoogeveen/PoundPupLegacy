namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DocumentableDocumentCreator : IEntityCreator<DocumentableDocument>
{
    public async Task CreateAsync(IAsyncEnumerable<DocumentableDocument> documentableDocuments, IDbConnection connection)
    {

        await using var documentableDocumentWriter = await DocumentableDocumentInserter.CreateAsync(connection);

        await foreach (var documentableDocument in documentableDocuments) {
            await documentableDocumentWriter.InsertAsync(documentableDocument);
        }

    }
}
