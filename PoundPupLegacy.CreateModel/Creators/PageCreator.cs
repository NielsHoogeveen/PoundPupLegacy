namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PageCreatorFactory(
        IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
        IDatabaseInserterFactory<EventuallyIdentifiableSearchable> searchableInserterFactory,
        IDatabaseInserterFactory<EventuallyIdentifiableSimpleTextNode> simpleTextNodeInserterFactory,
        IDatabaseInserterFactory<EventuallyIdentifiablePage> pageInserterFactory,
        NodeDetailsCreatorFactory nodeDetailsCreatorFactory
    ) : INodeCreatorFactory<EventuallyIdentifiablePage>
{
    public async Task<NodeCreator<EventuallyIdentifiablePage>> CreateAsync(IDbConnection connection) =>
        new(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await simpleTextNodeInserterFactory.CreateAsync(connection),
                await pageInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
