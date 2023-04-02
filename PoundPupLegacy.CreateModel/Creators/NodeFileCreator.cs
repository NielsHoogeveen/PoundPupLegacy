namespace PoundPupLegacy.CreateModel.Creators;

public class NodeFileCreator : IEntityCreator<NodeFile>
{
    public async Task CreateAsync(IAsyncEnumerable<NodeFile> nodeFiles, IDbConnection connection)
    {

        await using var nodeFileWriter = await NodeFileInserter.CreateAsync(connection);

        await foreach (var nodeFile in nodeFiles) {
            await nodeFileWriter.InsertAsync(nodeFile);
        }
    }
}
