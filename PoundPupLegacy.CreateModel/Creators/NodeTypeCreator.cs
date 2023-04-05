namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class NodeTypeCreator : EntityCreator<BasicNodeType>
{
    private readonly IDatabaseInserterFactory<NodeType> _nodeTypeInserterFactory;
    public NodeTypeCreator(
        IDatabaseInserterFactory<NodeType> nodeTypeInserterFactory)
    {
        _nodeTypeInserterFactory = nodeTypeInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<BasicNodeType> nodeTypes, IDbConnection connection)
    {

        await using var nodeTypeWriter = await _nodeTypeInserterFactory.CreateAsync(connection);

        await foreach (var nodeType in nodeTypes) {
            await nodeTypeWriter.InsertAsync(nodeType);
        }
    }
}
