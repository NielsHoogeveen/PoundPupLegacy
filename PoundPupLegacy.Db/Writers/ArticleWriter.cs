namespace PoundPupLegacy.Db.Writers;

internal sealed class ArticleWriter : IDatabaseWriter<Article>
{
    public static async Task<DatabaseWriter<Article>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<Article>(await SingleIdWriter.CreateSingleIdCommandAsync("article", connection));
    }
}
