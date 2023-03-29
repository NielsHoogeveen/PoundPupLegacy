namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class DocumentWriter : DatabaseWriter<Document>, IDatabaseWriter<Document>
{

    private const string ID = "id";
    private const string PUBLICATION_DATE = "publication_date";
    private const string PUBLICATION_DATE_RANGE = "publication_date_range";
    private const string SOURCE_URL = "source_url";
    private const string TEXT = "text";
    private const string TEASER = "teaser";
    private const string DOCUMENT_TYPE_ID = "document_type_id";
    public static async Task<DatabaseWriter<Document>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "document",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = SOURCE_URL,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = TEXT,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = TEASER,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = DOCUMENT_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PUBLICATION_DATE,
                    NpgsqlDbType = NpgsqlDbType.Unknown
                },
                new ColumnDefinition{
                    Name = PUBLICATION_DATE_RANGE,
                    NpgsqlDbType = NpgsqlDbType.Unknown
                },
            }
        );
        return new DocumentWriter(command);
    }

    internal DocumentWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(Document document)
    {
        if (document.Id is null)
            throw new NullReferenceException();
        WriteValue(document.Id, ID);
        WriteValue(document.Text, TEXT);
        WriteValue(document.Teaser, TEASER);
        WriteDateTimeRange(document.PublicationDate, PUBLICATION_DATE, PUBLICATION_DATE_RANGE);
        WriteNullableValue(document.SourceUrl, SOURCE_URL);
        WriteNullableValue(document.DocumentTypeId, DOCUMENT_TYPE_ID);
        await _command.ExecuteNonQueryAsync();
    }

}
