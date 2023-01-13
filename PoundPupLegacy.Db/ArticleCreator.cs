using PoundPupLegacy.Model;
using System.Runtime.InteropServices;

namespace PoundPupLegacy.Db;

public class ArticleCreator : IEntityCreator<Article>
{
    public static async Task CreateAsync(IAsyncEnumerable<Article> articles, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var simpleTextNodeWriter = await SimpleTextNodeWriter.CreateAsync(connection);
        await using var articleWriter = await ArticleWriter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var article in articles)
        {
            await nodeWriter.WriteAsync(article);
            await simpleTextNodeWriter.WriteAsync(article);
            await articleWriter.WriteAsync(article);
            foreach (var tenantNode in article.TenantNodes)
            {
                tenantNode.NodeId = article.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
