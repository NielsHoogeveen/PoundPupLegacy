﻿namespace PoundPupLegacy.CreateModel.Creators;

public class ArticleCreator : IEntityCreator<Article>
{
    public static async Task CreateAsync(IAsyncEnumerable<Article> articles, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var simpleTextNodeWriter = await SimpleTextNodeInserter.CreateAsync(connection);
        await using var articleWriter = await ArticleInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var article in articles) {
            await nodeWriter.InsertAsync(article);
            await searchableWriter.InsertAsync(article);
            await simpleTextNodeWriter.InsertAsync(article);
            await articleWriter.InsertAsync(article);
            foreach (var tenantNode in article.TenantNodes) {
                tenantNode.NodeId = article.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}