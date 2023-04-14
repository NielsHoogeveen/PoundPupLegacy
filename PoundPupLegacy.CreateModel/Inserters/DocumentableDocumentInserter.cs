namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = DocumentableDocumentInserterFactory;
using Request = DocumentableDocument;
using Inserter = DocumentableDocumentInserter;

internal sealed class DocumentableDocumentInserterFactory : DatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter DocumentableId = new() { Name = "documentable_id" };
    internal static NonNullableIntegerDatabaseParameter DocumentId = new() { Name = "document_id" };

    public override string TableName => "documentable_document";
}
internal sealed class DocumentableDocumentInserter : DatabaseInserter<Request>
{
    public DocumentableDocumentInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.DocumentableId, request.DocumentableId),
            ParameterValue.Create(Factory.DocumentId, request.DocumentId),
        };
    }
}
