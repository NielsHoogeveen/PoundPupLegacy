namespace PoundPupLegacy.Db.Writers;

internal class ArticleWriter : DatabaseWriter<Article>, IDatabaseWriter<Article>
{
    private const string ID = "id";
    private const string TEXT = "text";
    public static DatabaseWriter<Article> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
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

    internal override void Write(Article article)
    {
        if (article.Id is null)
            throw new NullReferenceException();
        WriteValue(article.Id, ID);
        WriteValue(article.Text, TEXT);
        _command.ExecuteNonQuery();
    }
}
