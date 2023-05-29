namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DocumentCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<SimpleTextNodeToCreate> simpleTextNodeInserterFactory,
    IDatabaseInserterFactory<Document.DocumentToCreate> documentInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<Document.DocumentToCreate>
{
    public async Task<IEntityCreator<Document.DocumentToCreate>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<Document.DocumentToCreate>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await simpleTextNodeInserterFactory.CreateAsync(connection),
                await documentInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );

}

