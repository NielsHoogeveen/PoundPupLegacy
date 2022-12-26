using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Model;


namespace PoundPupLegacy.Db.Writers;

internal class DocumentWriter : DatabaseWriter<Document>, IDatabaseWriter<Document>
{

    private const string ID = "id";
    private const string PUBLICATION_DATE = "publication_date";
    private const string PUBLICATION_DATE_RANGE = "publication_date_range";
    private const string SOURCE_URL = "source_url";
    private const string TEXT = "text";
    private const string DOCUMENT_TYPE_ID = "document_type_id";
    public static DatabaseWriter<Document> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
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

    internal override void Write(Document document)
    {
        WriteValue(document.Id, ID);
        WriteValue(document.Text, TEXT);
        WriteDateTimeRange(document.PublicationDate, PUBLICATION_DATE, PUBLICATION_DATE_RANGE);
        WriteNullableValue(document.SourceUrl, SOURCE_URL);
        WriteNullableValue(document.DocumentTypeId, DOCUMENT_TYPE_ID);
        _command.ExecuteNonQuery();
    }

}
