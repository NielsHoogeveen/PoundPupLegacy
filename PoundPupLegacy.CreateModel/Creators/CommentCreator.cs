namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CommentCreator : EntityCreator<Comment>
{
    private readonly IDatabaseInserterFactory<Comment> _commentInserterFactory;
    public CommentCreator(IDatabaseInserterFactory<Comment> commentInserterFactory)
    {
        _commentInserterFactory = commentInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<Comment> comments, IDbConnection connection)
    {

        await using var commentWriter = await _commentInserterFactory.CreateAsync(connection);

        await foreach (var comment in comments) {
            await commentWriter.InsertAsync(comment);
        }
    }
}
