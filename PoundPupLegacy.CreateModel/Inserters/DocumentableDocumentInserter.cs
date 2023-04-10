﻿namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class DocumentableDocumentInserterFactory : BasicDatabaseInserterFactory<DocumentableDocument, DocumentableDocumentInserter>
{
    internal static NonNullableIntegerDatabaseParameter DocumentableId = new() { Name = "documentable_id" };
    internal static NonNullableIntegerDatabaseParameter DocumentId = new() { Name = "document_id" };

    public override string TableName => "documentable_document";
}
internal sealed class DocumentableDocumentInserter : BasicDatabaseInserter<DocumentableDocument>
{
    public DocumentableDocumentInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(DocumentableDocument item)
    {
        return new ParameterValue[] {
            ParameterValue.Create(DocumentableDocumentInserterFactory.DocumentableId, item.DocumentableId),
            ParameterValue.Create(DocumentableDocumentInserterFactory.DocumentId, item.DocumentId),
        };
    }
}
