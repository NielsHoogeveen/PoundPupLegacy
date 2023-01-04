namespace PoundPupLegacy.Db.Writers;

internal class ArticleWriter : DatabaseWriter<Article>, IDatabaseWriter<Article>
{
    private const string ID = "id";
    private const string TEXT = "text";
    public static async Task<DatabaseWriter<Article>>    CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "article",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = TEXT,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new ArticleWriter(command);

    }

    internal ArticleWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(Article article)
    {
        if (article.Id is null)
            throw new NullReferenceException();
        WriteValue(article.Id, ID);
        WriteValue(article.Text, TEXT);
        await _command.ExecuteNonQueryAsync();
    }
}
