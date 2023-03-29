namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class DocumentTypeInserter : IDatabaseInserter<DocumentType>
{
    public static async Task<DatabaseInserter<DocumentType>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<DocumentType>("document_type", connection);
    }
}
