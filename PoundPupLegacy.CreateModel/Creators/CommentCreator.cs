namespace PoundPupLegacy.CreateModel.Creators;

public class CommentCreator : IEntityCreator<Comment>
{
    public async Task CreateAsync(IAsyncEnumerable<Comment> comments, IDbConnection connection)
    {

        await using var commentWriter = await CommentInserter.CreateAsync(connection);

        await foreach (var comment in comments) {
            await commentWriter.InsertAsync(comment);
        }
    }
}
