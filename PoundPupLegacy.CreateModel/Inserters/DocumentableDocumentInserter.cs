namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class DocumentableDocumentInserterFactory : DatabaseInserterFactory<DocumentableDocument, DocumentableDocumentInserter>
{
    internal static NonNullableIntegerDatabaseParameter DocumentableId = new() { Name = "documentable_id" };
    internal static NonNullableIntegerDatabaseParameter DocumentId = new() { Name = "document_id" };

    public override string TableName => "documentable_document";
}
internal sealed class DocumentableDocumentInserter : DatabaseInserter<DocumentableDocument>
{
    public DocumentableDocumentInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(DocumentableDocument item)
    {
        return new ParameterValue[] {
            ParameterValue.Create(DocumentableDocumentInserterFactory.DocumentableId, item.DocumentableId),
            ParameterValue.Create(DocumentableDocumentInserterFactory.DocumentId, item.DocumentId),
        };
    }
}
