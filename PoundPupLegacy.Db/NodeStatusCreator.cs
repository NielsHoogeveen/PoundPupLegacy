namespace PoundPupLegacy.Db;

public class NodeStatusCreator : IEntityCreator<NodeStatus>
{
    public static void Create(IEnumerable<NodeStatus> nodeStatuses, NpgsqlConnection connection)
    {

        using var nodeStatusWriter = NodeStatusWriter.Create(connection);

        foreach (var nodeStatus in nodeStatuses)
        {
            nodeStatusWriter.Write(nodeStatus);
        }
    }
}
