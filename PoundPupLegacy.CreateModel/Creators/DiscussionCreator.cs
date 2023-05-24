namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DiscussionCreatorFactory(
        IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
        IDatabaseInserterFactory<EventuallyIdentifiableSearchable> searchableInserterFactory,
        IDatabaseInserterFactory<EventuallyIdentifiableSimpleTextNode> simpleTextNodeInserterFactory,
        IDatabaseInserterFactory<EventuallyIdentifiableDiscussion> discussionInserterFactory,
        NodeDetailsCreatorFactory nodeDetailsCreatorFactory
    ) : INodeCreatorFactory<EventuallyIdentifiableDiscussion>
{
    public async Task<NodeCreator<EventuallyIdentifiableDiscussion>> CreateAsync(IDbConnection connection) =>
        new(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await simpleTextNodeInserterFactory.CreateAsync(connection),
                await discussionInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
