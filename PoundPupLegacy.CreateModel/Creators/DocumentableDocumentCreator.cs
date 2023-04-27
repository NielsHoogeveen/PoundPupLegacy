namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DocumentableDocumentCreator : EntityCreator<DocumentableDocument>
{
    private readonly IDatabaseInserterFactory<NodeTerm> _nodeTermInserterFactory;
    private readonly ISingleItemDatabaseReaderFactory<TermReaderByNameableIdRequest, Term> _termReaderByNameableIdFactory;
    public DocumentableDocumentCreator(
        IDatabaseInserterFactory<NodeTerm> nodeTermInserterFactory,
        ISingleItemDatabaseReaderFactory<TermReaderByNameableIdRequest, Term> termReaderByNameableIdFactory

    )
    {
        _nodeTermInserterFactory = nodeTermInserterFactory;
        _termReaderByNameableIdFactory = termReaderByNameableIdFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<DocumentableDocument> documentableDocuments, IDbConnection connection)
    {

        await using var nodeTermWriter = await _nodeTermInserterFactory.CreateAsync(connection);
        await using var termReaderByNameableId = await _termReaderByNameableIdFactory.CreateAsync(connection);
        await foreach (var documentableDocument in documentableDocuments) {
            var term = await termReaderByNameableId.ReadAsync(new TermReaderByNameableIdRequest {
                OwnerId = Constants.PPL,
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
