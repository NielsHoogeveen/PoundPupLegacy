namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DocumentableDocumentCreator(
    IDatabaseInserterFactory<NodeTerm> nodeTermInserterFactory,
    ISingleItemDatabaseReaderFactory<TermReaderByNameableIdRequest, Term> termReaderByNameableIdFactory
) : EntityCreator<DocumentableDocument>
{
    public override async Task CreateAsync(IAsyncEnumerable<DocumentableDocument> documentableDocuments, IDbConnection connection)
    {
        await using var nodeTermWriter = await nodeTermInserterFactory.CreateAsync(connection);
        await using var termReaderByNameableId = await termReaderByNameableIdFactory.CreateAsync(connection);
        await foreach (var documentableDocument in documentableDocuments) {
            var term = await termReaderByNameableId.ReadAsync(new TermReaderByNameableIdRequest {
                OwnerId = Constants.OWNER_SYSTEM,
                NameableId = documentableDocument.DocumentableId,
                VocabularyName = Constants.VOCABULARY_TOPICS,
            });
            await nodeTermWriter.InsertAsync(new NodeTerm {
                NodeId = documentableDocument.DocumentId,
                TermId = (int)term!.Id!,
            });
        }
    }
}
