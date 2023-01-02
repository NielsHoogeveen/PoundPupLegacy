namespace PoundPupLegacy.Db;

public class SingleIdEnitityCreator
{
    public static void Create(IEnumerable<BasicNode> nodes, string tableName, NpgsqlConnection connection)
    {
        using var nodeWriter = NodeWriter.Create(connection);
        using var idTableWriter = SingleIdWriter.CreateSingleIdWriter(tableName, connection);

        foreach (var node in nodes)
        {
            nodeWriter.Write(node);
            idTableWriter.Write((int)node.Id!);
        }
    }
}
