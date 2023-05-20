using Microsoft.Extensions.Logging;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;

internal sealed class FetchBlogService(
    IDbConnection connection,
    ILogger<FetchBlogService> logger,
    ISingleItemDatabaseReaderFactory<BlogDocumentReaderRequest, Blog> blogDocumentReaderFactory
) : DatabaseService(connection, logger), IFetchBlogService
{
    public async Task<Blog> FetchBlog(int publisherId, int tenantId, int pageNumber, int pageSize)
    {

        var startIndex = (pageNumber - 1) * pageSize;
        return await WithConnection(async (connection) => {
            await using var reader = await blogDocumentReaderFactory.CreateAsync(connection);
            var blog = await reader.ReadAsync(new BlogDocumentReaderRequest {
                PublisherId = publisherId,
                TenantId = tenantId,
                StartIndex = startIndex,
                Length = pageSize
            });
            if (blog is null)
                return new Blog {
                    Name = string.Empty,
                    Entries = Array.Empty<BlogPostTeaser>(),
                    NumberOfEntries = 0,
                    Id = 0,
                };
            return blog;
        });
    }
}
