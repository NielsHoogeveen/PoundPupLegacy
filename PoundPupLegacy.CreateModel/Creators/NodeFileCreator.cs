namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class NodeFileCreator : EntityCreator<NodeFile>
{
    private readonly IDatabaseInserterFactory<NodeFile> _nodeFileInserterFactory;
    public NodeFileCreator(IDatabaseInserterFactory<NodeFile> nodeFileInserterFactory)
    {
        _nodeFileInserterFactory = nodeFileInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<NodeFile> nodeFiles, IDbConnection connection)
    {

        await using var nodeFileWriter = await _nodeFileInserterFactory.CreateAsync(connection);

        await foreach (var nodeFile in nodeFiles) {
            await nodeFileWriter.InsertAsync(nodeFile);
        }
    }
}
