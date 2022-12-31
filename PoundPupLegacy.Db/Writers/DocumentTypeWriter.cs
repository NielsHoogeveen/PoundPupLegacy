namespace PoundPupLegacy.Db.Writers;

internal class DocumentTypeWriter : IDatabaseWriter<DocumentType>
{
    public static DatabaseWriter<DocumentType> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<DocumentType>(SingleIdWriter.CreateSingleIdCommand("document_type", connection));
    }
}
