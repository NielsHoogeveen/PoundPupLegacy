using Microsoft.Extensions.Logging;
using Npgsql;
using PoundPupLegacy.ViewModel.Readers;
using System.Diagnostics;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;

internal sealed class FetchNodeService<T>(
    NpgsqlDataSource dataSource,
    ILogger<FetchNodeService<T>> logger,
    ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, T> nodeDocumentReaderFactory
) : DatabaseService(dataSource, logger), IFetchNodeService<T>
    where T: class, Node
{
    public async Task<T?> FetchNode(int nodeId, int userId, int tenantId)
    {

        return await WithConnection(async (connection) => {
            var sw = new Stopwatch();
            sw.Start();
            await using var reader = await nodeDocumentReaderFactory.CreateAsync(connection);
            var result = await reader.ReadAsync(new NodeDocumentReaderRequest {
                NodeId = nodeId,
                UserId = userId,
                TenantId = tenantId
            });
            logger.LogInformation($"Fetching {typeof(T).Name} {nodeId} in {sw.ElapsedMilliseconds} ms");
            return result;
        });
    }
}
