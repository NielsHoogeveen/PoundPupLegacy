namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DocumentCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<SimpleTextNodeToCreate> simpleTextNodeInserterFactory,
    IDatabaseInserterFactory<Document.ToCreate> documentInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<Document.ToCreate>
{
    public async Task<IEntityCreator<Document.ToCreate>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<Document.ToCreate>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await simpleTextNodeInserterFactory.CreateAsync(connection),
                await documentInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );

}

