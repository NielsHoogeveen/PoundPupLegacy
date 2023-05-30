namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PageCreatorFactory(
        IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
        IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
        IDatabaseInserterFactory<SimpleTextNodeToCreate> simpleTextNodeInserterFactory,
        IDatabaseInserterFactory<Page.ToCreate> pageInserterFactory,
        NodeDetailsCreatorFactory nodeDetailsCreatorFactory
    ) : IEntityCreatorFactory<Page.ToCreate>
{
    public async Task<IEntityCreator<Page.ToCreate>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<Page.ToCreate>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await simpleTextNodeInserterFactory.CreateAsync(connection),
                await pageInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
