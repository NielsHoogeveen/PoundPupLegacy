namespace PoundPupLegacy.CreateModel.Inserters;

using Request = DocumentableDocument;

internal sealed class DocumentableDocumentInserterFactory : BasicDatabaseInserterFactory<Request>
{
    internal static NonNullableIntegerDatabaseParameter DocumentableId = new() { Name = "documentable_id" };
    internal static NonNullableIntegerDatabaseParameter DocumentId = new() { Name = "document_id" };

    public override string TableName => "documentable_document";

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(DocumentableId, request.DocumentableId),
            ParameterValue.Create(DocumentId, request.DocumentId),
        };
    }
}
