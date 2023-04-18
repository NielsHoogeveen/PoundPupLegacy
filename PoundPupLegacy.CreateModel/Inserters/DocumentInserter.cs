namespace PoundPupLegacy.CreateModel.Inserters;

using Request = Document;

internal sealed class DocumentInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NullableFuzzyDateDatabaseParameter Published = new() { Name = "published" };
    internal static NullableStringDatabaseParameter SourceUrl = new() { Name = "source_url" };
    internal static NonNullableStringDatabaseParameter Text = new() { Name = "text" };
    internal static NonNullableStringDatabaseParameter Teaser = new() { Name = "teaser" };
    internal static NullableIntegerDatabaseParameter DocumentTypeId = new() { Name = "document_type_id" };

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
