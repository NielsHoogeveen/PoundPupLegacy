namespace PoundPupLegacy.CreateModel.Inserters;

using Request = Document;

internal sealed class DocumentInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NullableFuzzyDateDatabaseParameter Published = new() { Name = "published" };
    private static readonly NullableStringDatabaseParameter SourceUrl = new() { Name = "source_url" };
    private static readonly NonNullableStringDatabaseParameter Text = new() { Name = "text" };
    private static readonly NonNullableStringDatabaseParameter Teaser = new() { Name = "teaser" };
    private static readonly NullableIntegerDatabaseParameter DocumentTypeId = new() { Name = "document_type_id" };

    public override string TableName => "document";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Text, request.Text),
            ParameterValue.Create(Teaser, request.Teaser),
            ParameterValue.Create(Published, request.PublicationDate),
            ParameterValue.Create(SourceUrl, request.SourceUrl),
            ParameterValue.Create(DocumentTypeId, request.DocumentTypeId),
        };
    }
}
