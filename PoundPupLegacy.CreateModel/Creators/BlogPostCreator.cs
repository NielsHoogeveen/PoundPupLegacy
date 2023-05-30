namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BlogPostCreatorFactory(
        IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
        IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
        IDatabaseInserterFactory<SimpleTextNodeToCreate> simpleTextNodeInserterFactory,
        IDatabaseInserterFactory<BlogPost.ToCreate> blogPostInserterFactory,
        NodeDetailsCreatorFactory nodeDetailsCreatorFactory
    ) : IEntityCreatorFactory<BlogPost.ToCreate>
{
    public async Task<IEntityCreator<BlogPost.ToCreate>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<BlogPost.ToCreate>(
            new () {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await simpleTextNodeInserterFactory.CreateAsync(connection),
                await blogPostInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
