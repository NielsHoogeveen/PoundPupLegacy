namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BlogPostCreatorFactory(
        IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
        IDatabaseInserterFactory<EventuallyIdentifiableSearchable> searchableInserterFactory,
        IDatabaseInserterFactory<EventuallyIdentifiableSimpleTextNode> simpleTextNodeInserterFactory,
        IDatabaseInserterFactory<EventuallyIdentifiableBlogPost> blogPostInserterFactory,
        NodeDetailsCreatorFactory nodeDetailsCreatorFactory
    ) : INodeCreatorFactory<EventuallyIdentifiableBlogPost>
{
    public async Task<NodeCreator<EventuallyIdentifiableBlogPost>> CreateAsync(IDbConnection connection) =>
        new (
            new () {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await simpleTextNodeInserterFactory.CreateAsync(connection),
                await blogPostInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
