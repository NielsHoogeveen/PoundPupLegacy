namespace PoundPupLegacy.CreateModel.Inserters;

using Request = Document.DocumentToCreate;

internal sealed class DocumentInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NullableFuzzyDateDatabaseParameter Published = new() { Name = "published" };
    private static readonly NullableStringDatabaseParameter SourceUrl = new() { Name = "source_url" };
    private static readonly NullableIntegerDatabaseParameter DocumentTypeId = new() { Name = "document_type_id" };

    public override string TableName => "document";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Published, request.DocumentDetails.Published),
            ParameterValue.Create(SourceUrl, request.DocumentDetails.SourceUrl),
            ParameterValue.Create(DocumentTypeId, request.DocumentDetails.DocumentTypeId),
        };
    }
}
