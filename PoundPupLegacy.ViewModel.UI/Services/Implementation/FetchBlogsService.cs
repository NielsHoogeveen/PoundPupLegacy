﻿using Microsoft.Extensions.Logging;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;
internal sealed class FetchBlogsService(
        IDbConnection connection,
        ILogger<FetchBlogService> logger,
        ISingleItemDatabaseReaderFactory<BlogsDocumentReaderRequest, List<BlogListEntry>> blogsDocumentReaderFactory
    ) : DatabaseService(connection, logger), IFetchBlogsService
{

    public async Task<List<BlogListEntry>> FetchBlogs(int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await blogsDocumentReaderFactory.CreateAsync(connection);
            var blogs = await reader.ReadAsync(new BlogsDocumentReaderRequest { TenantId = tenantId });
            if (blogs is not null)
                return blogs;
            return new List<BlogListEntry>();
        });
    }
}
