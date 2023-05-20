namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CommentCreator(IDatabaseInserterFactory<Comment> commentInserterFactory) : EntityCreator<Comment>
{
    public override async Task CreateAsync(IAsyncEnumerable<Comment> comments, IDbConnection connection)
    {
        await using var commentWriter = await commentInserterFactory.CreateAsync(connection);

        await foreach (var comment in comments) {
            await commentWriter.InsertAsync(comment);
        }
    }
}
