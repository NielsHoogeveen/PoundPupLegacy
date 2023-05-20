namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class NodeFileCreator(IDatabaseInserterFactory<NodeFile> nodeFileInserterFactory) : EntityCreator<NodeFile>
{
    public override async Task CreateAsync(IAsyncEnumerable<NodeFile> nodeFiles, IDbConnection connection)
    {
        await using var nodeFileWriter = await nodeFileInserterFactory.CreateAsync(connection);

        await foreach (var nodeFile in nodeFiles) {
            await nodeFileWriter.InsertAsync(nodeFile);
        }
    }
}
