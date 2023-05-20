using Microsoft.Extensions.Logging;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;

internal sealed class FetchNodeService(
    IDbConnection connection,
    ILogger<FetchNodeService> logger,
    ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, Node> nodeDocumentReaderFactory
) : DatabaseService(connection, logger), IFetchNodeService
{
    public async Task<Node?> FetchNode(int urlId, int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await nodeDocumentReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeDocumentReaderRequest {
                UrlId = urlId,
                UserId = userId,
                TenantId = tenantId
            });
        });
    }
}
