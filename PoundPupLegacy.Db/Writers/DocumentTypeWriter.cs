namespace PoundPupLegacy.Db.Writers;

internal class DocumentTypeWriter : IDatabaseWriter<DocumentType>
{
    public static async Task<DatabaseWriter<DocumentType>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<DocumentType>(await SingleIdWriter.CreateSingleIdCommandAsync("document_type", connection));
    }
}
