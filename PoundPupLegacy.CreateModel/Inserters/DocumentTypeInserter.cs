namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class DocumentTypeInserterFactory : SingleIdInserterFactory<DocumentType.DocumentTypeToCreate>
{
    protected override string TableName => "document_type";

    protected override bool AutoGenerateIdentity => false;

}
