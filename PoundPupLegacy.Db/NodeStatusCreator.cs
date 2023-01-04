namespace PoundPupLegacy.Db;

public class NodeStatusCreator : IEntityCreator<NodeStatus>
{
    public static async Task CreateAsync(IAsyncEnumerable<NodeStatus> nodeStatuses, NpgsqlConnection connection)
    {

        await using var nodeStatusWriter = await NodeStatusWriter.CreateAsync(connection);

        await foreach (var nodeStatus in nodeStatuses)
        {
            await nodeStatusWriter.WriteAsync(nodeStatus);
        }
    }
}
