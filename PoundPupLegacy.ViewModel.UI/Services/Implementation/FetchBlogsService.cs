using Microsoft.Extensions.Logging;
using Npgsql;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;
internal sealed class FetchBlogsService(
        NpgsqlDataSource dataSource,
        ILogger<FetchBlogService> logger,
        ISingleItemDatabaseReaderFactory<BlogsDocumentReaderRequest, List<BlogListEntry>> blogsDocumentReaderFactory
    ) : DatabaseService(dataSource, logger), IFetchBlogsService
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
