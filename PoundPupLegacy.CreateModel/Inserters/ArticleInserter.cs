namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class ArticleInserter : IDatabaseInserter<Article>
{
    public static async Task<DatabaseInserter<Article>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<Article>("article", connection);
    }
}
