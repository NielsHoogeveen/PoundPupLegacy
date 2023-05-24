namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DocumentCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSearchable> searchableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSimpleTextNode> simpleTextNodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableDocument> documentInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    IEntityCreatorFactory<DocumentableDocument, DocumentableDocumentCreator> documentableDocumentCreatorFactory
) : INodeCreatorFactory<EventuallyIdentifiableDocument>
{
    public async Task<NodeCreator<EventuallyIdentifiableDocument>> CreateAsync(IDbConnection connection) =>
        new DocumentCreator(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await simpleTextNodeInserterFactory.CreateAsync(connection),
                await documentInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await documentableDocumentCreatorFactory.CreateAsync(connection)
        );

}

public class DocumentCreator(
    List<IDatabaseInserter<EventuallyIdentifiableDocument>> inserters,
    NodeDetailsCreator nodeDetailsCreator,
    IEntityCreator<DocumentableDocument> documentableDocumentCreator
    ) : NodeCreator<EventuallyIdentifiableDocument>(inserters, nodeDetailsCreator)
{
    public override async Task ProcessAsync(EventuallyIdentifiableDocument element)
    {
        await base.ProcessAsync(element);
        foreach (var documentable in element.Documentables) {
            await documentableDocumentCreator.CreateAsync(new DocumentableDocument 
            { 
                DocumentableId = documentable, 
                DocumentId = element.Id!.Value 
            });
        }
    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await documentableDocumentCreator.DisposeAsync();
    }
}