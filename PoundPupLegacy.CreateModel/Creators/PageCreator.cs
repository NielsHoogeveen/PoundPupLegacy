namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PageCreatorFactory(
        IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
        IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
        IDatabaseInserterFactory<SimpleTextNodeToCreate> simpleTextNodeInserterFactory,
        IDatabaseInserterFactory<EventuallyIdentifiablePage> pageInserterFactory,
        NodeDetailsCreatorFactory nodeDetailsCreatorFactory
    ) : IEntityCreatorFactory<EventuallyIdentifiablePage>
{
    public async Task<IEntityCreator<EventuallyIdentifiablePage>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<EventuallyIdentifiablePage>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await simpleTextNodeInserterFactory.CreateAsync(connection),
                await pageInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
