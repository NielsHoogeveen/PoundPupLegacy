namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class DocumentableDocumentWriter : DatabaseWriter<DocumentableDocument>, IDatabaseWriter<DocumentableDocument>
{

    private const string DOCUMENTABLE_ID = "documentable_id";
    private const string DOCUMENT_ID = "document_id";
    public static async Task<DatabaseWriter<DocumentableDocument>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "documentable_document",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = DOCUMENTABLE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = DOCUMENT_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new DocumentableDocumentWriter(command);

    }

    internal DocumentableDocumentWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(DocumentableDocument documentableDocument)
    {
        WriteValue(documentableDocument.DocumentableId, DOCUMENTABLE_ID);
        WriteNullableValue(documentableDocument.DocumentId, DOCUMENT_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
