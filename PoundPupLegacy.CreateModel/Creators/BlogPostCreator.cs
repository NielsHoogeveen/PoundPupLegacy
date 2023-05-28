namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BlogPostCreatorFactory(
        IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
        IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
        IDatabaseInserterFactory<SimpleTextNodeToCreate> simpleTextNodeInserterFactory,
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
