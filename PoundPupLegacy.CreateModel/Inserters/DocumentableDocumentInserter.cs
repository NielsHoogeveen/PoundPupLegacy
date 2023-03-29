namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class DocumentableDocumentInserter : DatabaseInserter<DocumentableDocument>, IDatabaseInserter<DocumentableDocument>
{

    private const string DOCUMENTABLE_ID = "documentable_id";
    private const string DOCUMENT_ID = "document_id";
    public static async Task<DatabaseInserter<DocumentableDocument>> CreateAsync(NpgsqlConnection connection)
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
        return new DocumentableDocumentInserter(command);

    }

    internal DocumentableDocumentInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(DocumentableDocument documentableDocument)
    {
        WriteValue(documentableDocument.DocumentableId, DOCUMENTABLE_ID);
        WriteNullableValue(documentableDocument.DocumentId, DOCUMENT_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
