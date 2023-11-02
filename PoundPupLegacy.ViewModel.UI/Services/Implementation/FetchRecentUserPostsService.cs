using Microsoft.Extensions.Logging;
using Npgsql;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;
internal sealed class FetchRecentUserPostsService(
    NpgsqlDataSource dataSource,
    ILogger<FetchRecentUserPostsService> logger,
    ISingleItemDatabaseReaderFactory<RecentUserPostsDocumentReaderRequest, RecentPosts> casesDocumentReaderFactory
) : DatabaseService(dataSource,logger), IFetchRecentUserPostsService
{
    public async Task<RecentPosts> FetchRecentUserPosts(int pageSize, int pageNumber, int tenantId, int userId, int userIdPublisher)
    {
        var startIndex = (pageNumber - 1) * pageSize;

        return await WithConnection(async (connection) => {
            await using var reader = await casesDocumentReaderFactory.CreateAsync(connection);
            var cases = await reader.ReadAsync(new RecentUserPostsDocumentReaderRequest {
                Limit = pageSize,
                Offset = startIndex,
                TenantId = tenantId,
                UserId = userId,
                UserIdPublisher = userIdPublisher,
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
