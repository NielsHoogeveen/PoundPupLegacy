namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CommentCreatorFactory(
    IDatabaseInserterFactory<Comment> commentInserterFactory
) : IEntityCreatorFactory<Comment>
{
    public async Task<IEntityCreator<Comment>> CreateAsync(IDbConnection connection) => 
        new InsertingEntityCreator<Comment>(new () {
            await commentInserterFactory.CreateAsync(connection)
        });
}
