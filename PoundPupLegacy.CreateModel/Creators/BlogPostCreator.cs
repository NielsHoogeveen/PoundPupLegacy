namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BlogPostCreatorFactory(
        IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
        IDatabaseInserterFactory<EventuallyIdentifiableSearchable> searchableInserterFactory,
        IDatabaseInserterFactory<EventuallyIdentifiableSimpleTextNode> simpleTextNodeInserterFactory,
        IDatabaseInserterFactory<EventuallyIdentifiableBlogPost> blogPostInserterFactory,
        NodeDetailsCreatorFactory nodeDetailsCreatorFactory
    ) : IEntityCreatorFactory<EventuallyIdentifiableBlogPost>
{
    public async Task<IEntityCreator<EventuallyIdentifiableBlogPost>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<EventuallyIdentifiableBlogPost>(
            new () {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await simpleTextNodeInserterFactory.CreateAsync(connection),
                await blogPostInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
