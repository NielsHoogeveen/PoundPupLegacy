namespace PoundPupLegacy.Db;

public class NodeTypeCreator : IEntityCreator<NodeType>
{
    public static async Task CreateAsync(IAsyncEnumerable<NodeType> nodeTypes, NpgsqlConnection connection)
    {

        await using var nodeTypeWriter = await NodeTypeWriter.CreateAsync(connection);

        await foreach (var nodeType in nodeTypes)
        {
            await nodeTypeWriter.WriteAsync(nodeType);
        }
    }
}
