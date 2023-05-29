namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DiscussionCreatorFactory(
        IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
        IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
        IDatabaseInserterFactory<SimpleTextNodeToCreate> simpleTextNodeInserterFactory,
        IDatabaseInserterFactory<Discussion.DiscussionToCreate> discussionInserterFactory,
        NodeDetailsCreatorFactory nodeDetailsCreatorFactory
    ) : IEntityCreatorFactory<Discussion.DiscussionToCreate>
{
    public async Task<IEntityCreator<Discussion.DiscussionToCreate>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<Discussion.DiscussionToCreate>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await simpleTextNodeInserterFactory.CreateAsync(connection),
                await discussionInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
