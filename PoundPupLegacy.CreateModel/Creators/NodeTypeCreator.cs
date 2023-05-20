namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class NodeTypeCreator(IDatabaseInserterFactory<NodeType> nodeTypeInserterFactory) : EntityCreator<BasicNodeType>
{
    public override async Task CreateAsync(IAsyncEnumerable<BasicNodeType> nodeTypes, IDbConnection connection)
    {
        await using var nodeTypeWriter = await nodeTypeInserterFactory.CreateAsync(connection);

        await foreach (var nodeType in nodeTypes) {
            await nodeTypeWriter.InsertAsync(nodeType);
        }
    }
}
