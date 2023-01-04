namespace PoundPupLegacy.Db;

public class SingleIdEnitityCreator
{
    public static async Task Create(IEnumerable<BasicNode> nodes, string tableName, NpgsqlConnection connection)
    {
        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var idTableWriter = await SingleIdWriter.CreateSingleIdWriterAsync(tableName, connection);

        foreach (var node in nodes)
        {
            await nodeWriter.WriteAsync(node);
            await idTableWriter.WriteAsync((int)node.Id!);
        }
    }
}
