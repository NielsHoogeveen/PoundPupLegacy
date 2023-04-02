namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class ArticleInserter : IDatabaseInserter<Article>
{
    public static async Task<DatabaseInserter<Article>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<Article>("article", connection);
    }
}
