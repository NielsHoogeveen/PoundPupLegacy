namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DocumentableDocumentCreator : EntityCreator<DocumentableDocument>
{
    private readonly IDatabaseInserterFactory<DocumentableDocument> _documentableDocumentInserterFactory;
    public DocumentableDocumentCreator(IDatabaseInserterFactory<DocumentableDocument> documentableDocumentInserterFactory)
    {
        _documentableDocumentInserterFactory = documentableDocumentInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<DocumentableDocument> documentableDocuments, IDbConnection connection)
    {

        await using var documentableDocumentWriter = await _documentableDocumentInserterFactory.CreateAsync(connection);

        await foreach (var documentableDocument in documentableDocuments) {
            await documentableDocumentWriter.InsertAsync(documentableDocument);
        }

    }
}
