namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class DocumentTypeWriter : IDatabaseWriter<DocumentType>
{
    public static async Task<DatabaseWriter<DocumentType>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<DocumentType>("document_type", connection);
    }
}
