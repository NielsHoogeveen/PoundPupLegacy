namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BlogPostCreatorFactory(
        IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
        IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
        IDatabaseInserterFactory<SimpleTextNodeToCreate> simpleTextNodeInserterFactory,
        IDatabaseInserterFactory<BlogPost.BlogPostToCreate> blogPostInserterFactory,
        NodeDetailsCreatorFactory nodeDetailsCreatorFactory
    ) : IEntityCreatorFactory<BlogPost.BlogPostToCreate>
{
    public async Task<IEntityCreator<BlogPost.BlogPostToCreate>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<BlogPost.BlogPostToCreate>(
            new () {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await simpleTextNodeInserterFactory.CreateAsync(connection),
                await blogPostInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
