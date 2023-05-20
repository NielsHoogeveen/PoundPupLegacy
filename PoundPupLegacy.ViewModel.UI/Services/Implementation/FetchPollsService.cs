using Microsoft.Extensions.Logging;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;

internal sealed class FetchPollsService(
    IDbConnection connection,
    ILogger<FetchPollsService> logger,
    ISingleItemDatabaseReaderFactory<PollsDocumentReaderRequest, Polls> pollsDocumentReaderFactory
) : DatabaseService(connection, logger), IFetchPollsService
{

    public async Task<Polls> GetPolls(int userId, int tenantId, int pageSize, int pageNumber)
    {
        var offset = (pageNumber - 1) * pageSize;

        return await WithConnection(async (connection) => {
            await using var reader = await pollsDocumentReaderFactory.CreateAsync(connection);
            var polls = await reader.ReadAsync(new PollsDocumentReaderRequest {
                UserId = userId,
                TenantId = tenantId,
                Limit = pageSize,
                Offset = offset
            });
            var result = polls is not null ? polls : new Polls {
                Entries = Array.Empty<PollListEntry>(),
                NumberOfEntries = 0
            };
            return result;
        });
    }
}
