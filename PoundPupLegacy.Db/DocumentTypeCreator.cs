namespace PoundPupLegacy.Db;

public class DocumentTypeCreator : IEntityCreator<DocumentType>
{
    public static void Create(IEnumerable<DocumentType> documentTypes, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var nameableWriter = NameableWriter.Create(connection);
        using var documentTypeWriter = DocumentTypeWriter.Create(connection);

        foreach (var documentType in documentTypes)
        {
            nodeWriter.Write(documentType);
            nameableWriter.Write(documentType);
            documentTypeWriter.Write(documentType);
        }
    }
}
