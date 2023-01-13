﻿using PoundPupLegacy.Model;
using System.Runtime.InteropServices;

namespace PoundPupLegacy.Db;

public class PageCreator : IEntityCreator<Page>
{
    public static async Task CreateAsync(IAsyncEnumerable<Page> pages, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var simpleTextNodeWriter = await SimpleTextNodeWriter.CreateAsync(connection);
        await using var pageWriter = await PageWriter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var page in pages)
        {
            await nodeWriter.WriteAsync(page);
            await simpleTextNodeWriter.WriteAsync(page);
            await pageWriter.WriteAsync(page);
            foreach (var tenantNode in page.TenantNodes)
            {
                tenantNode.NodeId = page.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
