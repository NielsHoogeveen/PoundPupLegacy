namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CommentCreatorFactory(
    IDatabaseInserterFactory<Comment> commentInserterFactory
) : IInsertingEntityCreatorFactory<Comment>
{
    public async Task<InsertingEntityCreator<Comment>> CreateAsync(IDbConnection connection) => 
        new (new () {
            await commentInserterFactory.CreateAsync(connection)
        });
}
