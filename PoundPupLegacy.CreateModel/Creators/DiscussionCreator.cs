namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DiscussionCreatorFactory(
        IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
        IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
        IDatabaseInserterFactory<SimpleTextNodeToCreate> simpleTextNodeInserterFactory,
        IDatabaseInserterFactory<EventuallyIdentifiableDiscussion> discussionInserterFactory,
        NodeDetailsCreatorFactory nodeDetailsCreatorFactory
    ) : IEntityCreatorFactory<EventuallyIdentifiableDiscussion>
{
    public async Task<IEntityCreator<EventuallyIdentifiableDiscussion>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<EventuallyIdentifiableDiscussion>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await simpleTextNodeInserterFactory.CreateAsync(connection),
                await discussionInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
