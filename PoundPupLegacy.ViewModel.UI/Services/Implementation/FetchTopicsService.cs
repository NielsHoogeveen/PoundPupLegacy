using Microsoft.Extensions.Logging;
using Npgsql;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;
using SearchOption = PoundPupLegacy.Common.SearchOption;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;

internal sealed class FetchTopicsService(
    NpgsqlDataSource dataSource,
    ILogger<FetchTopicsService> logger,
    ISingleItemDatabaseReaderFactory<TopicsDocumentReaderRequest, Topics> topicsDocumentReaderFactory
) : DatabaseService(dataSource, logger), IFetchTopicsService
{
    public async Task<Topics> FetchTopics(int userId, int tenantId, int pageSize, int pageNumber, string searchTerm, SearchOption searchOption)
    {
        var offset = (pageNumber - 1) * pageSize;

        return await WithConnection(async (connection) => {
            await using var reader = await topicsDocumentReaderFactory.CreateAsync(connection);
            var result = await reader.ReadAsync(new TopicsDocumentReaderRequest {
                UserId = userId,
                TenantId = tenantId,
                Limit = pageSize,
                Offset = offset,
                SearchTerm = searchTerm,
                SearchOption = searchOption
            });
            if (result is not null)
                return result;
            else
                return new Topics {
                    Entries = Array.Empty<TopicListEntry>(),
                    NumberOfEntries = 0
                };
        });
    }
}
