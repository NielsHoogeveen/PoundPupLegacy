using Microsoft.Extensions.Logging;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;

internal sealed class FetchSubgroupService(
    IDbConnection connection,
    ILogger<FetchSubgroupService> logger,
    ISingleItemDatabaseReaderFactory<SubgroupsDocumentReaderRequest, SubgroupPagedList> subgroupsDocumentReaderFactory
) : DatabaseService(connection, logger), IFetchSubgroupService
{
    public async Task<SubgroupPagedList> GetSubGroupPagedList(int userId, int subgroupId, int pageSize, int pageNumber)
    {
        var offset = (pageNumber - 1) * pageSize;
        return await WithConnection(async (connection) => {
            await using var reader = await subgroupsDocumentReaderFactory.CreateAsync(connection);
            var result = await reader.ReadAsync(new SubgroupsDocumentReaderRequest {
                UserId = userId,
                SubgroupId = subgroupId,
                Limit = pageSize,
                Offset = offset
            });
            if (result != null)
                return result;
            return new SubgroupPagedList {
                Entries = Array.Empty<SubgroupListEntry>(),
                NumberOfEntries = 0,
                Name = "",
                Description = ""

            };
        });
    }
}
