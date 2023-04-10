namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class DocumentInserterFactory : DatabaseInserterFactory<Document>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NullableFuzzyDateDatabaseParameter Published = new() { Name = "published" };
    internal static NullableStringDatabaseParameter SourceUrl = new() { Name = "source_url" };
    internal static NonNullableStringDatabaseParameter Text = new() { Name = "text" };
    internal static NonNullableStringDatabaseParameter Teaser = new() { Name = "teaser" };
    internal static NullableIntegerDatabaseParameter DocumentTypeId = new() { Name = "document_type_id" };

    public override async Task<IDatabaseInserter<Document>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "document",
            new DatabaseParameter[] {
                Id,
                Published,
                SourceUrl,
                Text,
                Teaser,
                DocumentTypeId
            }
        );
        return new DocumentInserter(command);
    }

}
internal sealed class DocumentInserter : DatabaseInserter<Document>
{
    internal DocumentInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Document document)
    {
        if (document.Id is null)
            throw new NullReferenceException();
        Set(DocumentInserterFactory.Id, document.Id.Value);
        Set(DocumentInserterFactory.Text, document.Text);
        Set(DocumentInserterFactory.Teaser, document.Teaser);
        Set(DocumentInserterFactory.Published, document.PublicationDate);
        Set(DocumentInserterFactory.SourceUrl, document.SourceUrl);
        Set(DocumentInserterFactory.DocumentTypeId, document.DocumentTypeId);
        await _command.ExecuteNonQueryAsync();
    }

}
