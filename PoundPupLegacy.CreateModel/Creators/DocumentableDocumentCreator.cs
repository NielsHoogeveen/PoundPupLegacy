namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DocumentableDocumentCreatorFactory(
    IDatabaseInserterFactory<NodeTerm> nodeTermInserterFactory,
    ISingleItemDatabaseReaderFactory<TermReaderByNameableIdRequest, Term> termReaderByNameableIdFactory
) : IEntityCreatorFactory<DocumentableDocument>
{
    public async Task<IEntityCreator<DocumentableDocument>> CreateAsync(IDbConnection connection) =>
        new DocumentableDocumentCreator(
            await nodeTermInserterFactory.CreateAsync(connection),
            await termReaderByNameableIdFactory.CreateAsync(connection)
        );
}

public class DocumentableDocumentCreator(
    IDatabaseInserter<NodeTerm> nodeTermInserter,
    ISingleItemDatabaseReader<TermReaderByNameableIdRequest, Term> termReader
) : EntityCreator<DocumentableDocument>
{
    public override async Task ProcessAsync(DocumentableDocument element)
    {
        await base.ProcessAsync(element);
        var term = await termReader.ReadAsync(new TermReaderByNameableIdRequest {
            OwnerId = Constants.OWNER_SYSTEM,
            NameableId = element.DocumentableId,
            VocabularyName = Constants.VOCABULARY_TOPICS,
        });
        await nodeTermInserter.InsertAsync(new NodeTerm {
            NodeId = element.DocumentId,
            TermId = (int)term!.Id!,
        });
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await nodeTermInserter.DisposeAsync();
        await termReader.DisposeAsync();
    }
}