namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DocumentCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<SimpleTextNodeToCreate> simpleTextNodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableDocument> documentInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiableDocument>
{
    public async Task<IEntityCreator<EventuallyIdentifiableDocument>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<EventuallyIdentifiableDocument>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await simpleTextNodeInserterFactory.CreateAsync(connection),
                await documentInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );

}

