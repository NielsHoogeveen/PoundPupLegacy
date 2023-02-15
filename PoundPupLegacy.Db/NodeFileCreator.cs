namespace PoundPupLegacy.Db;

public class NodeFileCreator : IEntityCreator<NodeFile>
{
    public static async Task CreateAsync(IAsyncEnumerable<NodeFile> nodeFiles, NpgsqlConnection connection)
    {

        await using var nodeFileWriter = await NodeFileWriter.CreateAsync(connection);

        await foreach (var nodeFile in nodeFiles)
        {
            await nodeFileWriter.WriteAsync(nodeFile);
        }
    }
}
