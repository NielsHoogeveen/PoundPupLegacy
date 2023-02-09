namespace PoundPupLegacy.Db;

public class DocumentableDocumentCreator : IEntityCreator<DocumentableDocument>
{
    public static async Task CreateAsync(IAsyncEnumerable<DocumentableDocument> documentableDocuments, NpgsqlConnection connection)
    {

        await using var documentableDocumentWriter = await DocumentableDocumentWriter.CreateAsync(connection);

        await foreach (var documentableDocument in documentableDocuments)
        {
            await documentableDocumentWriter.WriteAsync(documentableDocument);
        }

    }
}
