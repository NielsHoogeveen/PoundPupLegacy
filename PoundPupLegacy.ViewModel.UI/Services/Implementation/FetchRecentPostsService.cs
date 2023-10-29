using Microsoft.Extensions.Logging;
using Npgsql;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;
internal sealed class FetchRecentPostsService(
    NpgsqlDataSource dataSource,
    ILogger<FetchRecentPostsService> logger,
    ISingleItemDatabaseReaderFactory<RecentPostsDocumentReaderRequest, RecentPosts> casesDocumentReaderFactory
) : DatabaseService(dataSource,logger), IFetchRecentPostsService
{
    public async Task<RecentPosts> FetchRecentPosts(int pageSize, int pageNumber, int tenantId, int userId)
    {
        var startIndex = (pageNumber - 1) * pageSize;

        return await WithConnection(async (connection) => {
            await using var reader = await casesDocumentReaderFactory.CreateAsync(connection);
            var cases = await reader.ReadAsync(new RecentPostsDocumentReaderRequest {
                Limit = pageSize,
                Offset = startIndex,
                TenantId = tenantId,
                UserId = userId
            });
            var result = cases is not null
                ? cases
                : new RecentPosts {
                    Entries = Array.Empty<RecentPostListEntry>(),
                    NumberOfEntries = 0,
                };

            return result;
        });
    }
}
