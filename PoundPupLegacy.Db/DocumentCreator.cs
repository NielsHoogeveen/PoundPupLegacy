namespace PoundPupLegacy.Db;

public class DocumentCreator : IEntityCreator<Document>
{
    public static void Create(IEnumerable<Document> documents, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var documentWriter = DocumentWriter.Create(connection);

        foreach (var document in documents)
        {
            nodeWriter.Write(document);
            documentWriter.Write(document);
        }
    }
}
