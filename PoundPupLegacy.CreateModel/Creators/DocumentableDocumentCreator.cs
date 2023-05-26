namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DocumentableDocumentCreatorFactory(
    IDatabaseInserterFactory<NodeTerm> nodeTermInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermIdReaderByNameableIdRequest, int> termReaderByNameableIdFactory
) : IEntityCreatorFactory<DocumentableDocument>
{
    public async Task<IEntityCreator<DocumentableDocument>> CreateAsync(IDbConnection connection) =>
        new DocumentableDocumentCreator(
            await nodeTermInserterFactory.CreateAsync(connection),
            await nodeIdReaderFactory.CreateAsync(connection),
            await termReaderByNameableIdFactory.CreateAsync(connection)
        );
}

public class DocumentableDocumentCreator(
    IDatabaseInserter<NodeTerm> nodeTermInserter,
    IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
    IMandatorySingleItemDatabaseReader<TermIdReaderByNameableIdRequest, int> termReader
) : EntityCreator<DocumentableDocument>
{
    public override async Task ProcessAsync(DocumentableDocument element)
    {
        await base.ProcessAsync(element);
        var vocabularyIdTopics = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_TOPICS
        });
        var termId = await termReader.ReadAsync(new TermIdReaderByNameableIdRequest {
            NameableId = element.DocumentableId,
            VocabularyId = vocabularyIdTopics,
        });
        await nodeTermInserter.InsertAsync(new NodeTerm {
            NodeId = element.DocumentId,
            TermId = termId,
        });
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await nodeTermInserter.DisposeAsync();
        await termReader.DisposeAsync();
        await nodeIdReader.DisposeAsync();
    }
}