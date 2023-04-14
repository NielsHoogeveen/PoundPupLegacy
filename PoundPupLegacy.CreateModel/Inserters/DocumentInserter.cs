namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class DocumentInserterFactory : DatabaseInserterFactory<Document, DocumentInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NullableFuzzyDateDatabaseParameter Published = new() { Name = "published" };
    internal static NullableStringDatabaseParameter SourceUrl = new() { Name = "source_url" };
    internal static NonNullableStringDatabaseParameter Text = new() { Name = "text" };
    internal static NonNullableStringDatabaseParameter Teaser = new() { Name = "teaser" };
    internal static NullableIntegerDatabaseParameter DocumentTypeId = new() { Name = "document_type_id" };

    public override string TableName => "document";
}
internal sealed class DocumentInserter : DatabaseInserter<Document>
{
    public DocumentInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Document item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(DocumentInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(DocumentInserterFactory.Text, item.Text),
            ParameterValue.Create(DocumentInserterFactory.Teaser, item.Teaser),
            ParameterValue.Create(DocumentInserterFactory.Published, item.PublicationDate),
            ParameterValue.Create(DocumentInserterFactory.SourceUrl, item.SourceUrl),
            ParameterValue.Create(DocumentInserterFactory.DocumentTypeId, item.DocumentTypeId),
        };
    }
}
