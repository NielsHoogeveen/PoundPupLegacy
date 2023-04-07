namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class DocumentableDocumentInserterFactory : DatabaseInserterFactory<DocumentableDocument>
{
    public override async Task<IDatabaseInserter<DocumentableDocument>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "documentable_document",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = DocumentableDocumentInserter.DOCUMENTABLE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = DocumentableDocumentInserter.DOCUMENT_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new DocumentableDocumentInserter(command);
    }
}
internal sealed class DocumentableDocumentInserter : DatabaseInserter<DocumentableDocument>
{

    internal const string DOCUMENTABLE_ID = "documentable_id";
    internal const string DOCUMENT_ID = "document_id";

    internal DocumentableDocumentInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(DocumentableDocument documentableDocument)
    {
        SetParameter(documentableDocument.DocumentableId, DOCUMENTABLE_ID);
        SetNullableParameter(documentableDocument.DocumentId, DOCUMENT_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
