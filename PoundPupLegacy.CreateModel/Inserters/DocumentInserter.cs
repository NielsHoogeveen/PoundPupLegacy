using static System.Net.Mime.MediaTypeNames;

namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class DocumentInserterFactory : DatabaseInserterFactory<Document>
{
    public override async Task<IDatabaseInserter<Document>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "document",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = DocumentInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = DocumentInserter.SOURCE_URL,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = DocumentInserter.TEXT,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = DocumentInserter.TEASER,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = DocumentInserter.DOCUMENT_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = DocumentInserter.PUBLICATION_DATE,
                    NpgsqlDbType = NpgsqlDbType.Unknown
                },
                new ColumnDefinition{
                    Name = DocumentInserter.PUBLICATION_DATE_RANGE,
                    NpgsqlDbType = NpgsqlDbType.Unknown
                },
            }
        );
        return new DocumentInserter(command);
    }

}
internal sealed class DocumentInserter : DatabaseInserter<Document>
{

    internal const string ID = "id";
    internal const string PUBLICATION_DATE = "publication_date";
    internal const string PUBLICATION_DATE_RANGE = "publication_date_range";
    internal const string SOURCE_URL = "source_url";
    internal const string TEXT = "text";
    internal const string TEASER = "teaser";
    internal const string DOCUMENT_TYPE_ID = "document_type_id";

    internal DocumentInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Document document)
    {
        if (document.Id is null)
            throw new NullReferenceException();
        SetParameter(document.Id, ID);
        SetParameter(document.Text, TEXT);
        SetParameter(document.Teaser, TEASER);
        SetDateTimeRangeParameter(document.PublicationDate, PUBLICATION_DATE, PUBLICATION_DATE_RANGE);
        SetNullableParameter(document.SourceUrl, SOURCE_URL);
        SetNullableParameter(document.DocumentTypeId, DOCUMENT_TYPE_ID);
        await _command.ExecuteNonQueryAsync();
    }

}
