namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class DocumentableDocumentInserterFactory : DatabaseInserterFactory<DocumentableDocument>
{
    internal static NonNullableIntegerDatabaseParameter DocumentableId = new() { Name = "documentable_id" };
    internal static NonNullableIntegerDatabaseParameter DocumentId = new() { Name = "document_id" };

    public override async Task<IDatabaseInserter<DocumentableDocument>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "documentable_document",
            new DatabaseParameter[] {
                DocumentableId,
                DocumentId
            }
        );
        return new DocumentableDocumentInserter(command);
    }
}
internal sealed class DocumentableDocumentInserter : DatabaseInserter<DocumentableDocument>
{
    internal DocumentableDocumentInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(DocumentableDocument documentableDocument)
    {
        Set(DocumentableDocumentInserterFactory.DocumentableId, documentableDocument.DocumentableId);
        Set(DocumentableDocumentInserterFactory.DocumentId, documentableDocument.DocumentId);
        await _command.ExecuteNonQueryAsync();
    }
}
