namespace PoundPupLegacy.CreateModel.Creators;

public class CommentCreator : IEntityCreator<Comment>
{
    public static async Task CreateAsync(IAsyncEnumerable<Comment> comments, NpgsqlConnection connection)
    {

        await using var commentWriter = await CommentInserter.CreateAsync(connection);

        await foreach (var comment in comments) {
            await commentWriter.WriteAsync(comment);
        }
    }
}
