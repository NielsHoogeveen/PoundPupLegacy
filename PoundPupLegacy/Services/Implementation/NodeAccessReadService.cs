using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using PoundPupLegacy.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

public class NodeAccessReadService(
    NpgsqlDataSource dataSource,
    ILogger<NodeAccessReadService> logger,
    IEnumerableDatabaseReaderFactory<NodeAccessReaderRequest, NodeAccess> nodeAccessDatabaseReaderFactory
) : DatabaseService(dataSource, logger), INodeAccessReadService
{
    public async Task<List<NodeAccess>> ReadNodeAccess(int nodeId)
    {
        return await WithConnection(async connection => {
            await using var reader = await nodeAccessDatabaseReaderFactory.CreateAsync(connection);
            return await GetNodeAccesses(reader, nodeId);
        });
    }


    private async Task<List<NodeAccess>> GetNodeAccesses(IEnumerableDatabaseReader<NodeAccessReaderRequest, NodeAccess> reader, int nodeId)
    {
        var lst = new List<NodeAccess>();
        await foreach(var element in reader.ReadAsync(new NodeAccessReaderRequest { NodeId = nodeId })) {
            lst.Add(element);
        }
        return lst;
    }
}
