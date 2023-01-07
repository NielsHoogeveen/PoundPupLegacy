using System.Runtime.InteropServices;

namespace PoundPupLegacy.Db;

public class ArticleCreator : IEntityCreator<Article>
{
    public static async Task CreateAsync(IAsyncEnumerable<Article> articles, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var simpleTextNodeWriter = await SimpleTextNodeWriter.CreateAsync(connection);
        await using var articleWriter = await ArticleWriter.CreateAsync(connection);

        await foreach (var article in articles)
        {
            await nodeWriter.WriteAsync(article);
            await simpleTextNodeWriter.WriteAsync(article);
            await articleWriter.WriteAsync(article);
        }
    }
}
