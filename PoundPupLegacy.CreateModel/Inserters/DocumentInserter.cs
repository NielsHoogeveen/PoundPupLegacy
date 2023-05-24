namespace PoundPupLegacy.CreateModel.Inserters;

using Request = NewDocument;

internal sealed class DocumentInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NullableFuzzyDateDatabaseParameter Published = new() { Name = "published" };
    private static readonly NullableStringDatabaseParameter SourceUrl = new() { Name = "source_url" };
    private static readonly NullableIntegerDatabaseParameter DocumentTypeId = new() { Name = "document_type_id" };

    public override string TableName => "document";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Published, request.Published),
            ParameterValue.Create(SourceUrl, request.SourceUrl),
            ParameterValue.Create(DocumentTypeId, request.DocumentTypeId),
        };
    }
}
