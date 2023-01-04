namespace PoundPupLegacy.Db;

public class NodeStatusCreator : IEntityCreator<NodeStatus>
{
    public static async Task CreateAsync(IEnumerable<NodeStatus> nodeStatuses, NpgsqlConnection connection)
    {

        await using var nodeStatusWriter = await NodeStatusWriter.CreateAsync(connection);

        foreach (var nodeStatus in nodeStatuses)
        {
            await nodeStatusWriter.WriteAsync(nodeStatus);
        }
    }
}
