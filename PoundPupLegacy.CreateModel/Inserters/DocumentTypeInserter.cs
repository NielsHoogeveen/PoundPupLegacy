namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class DocumentTypeInserterFactory : SingleIdInserterFactory<DocumentType.ToCreate>
{
    protected override string TableName => "document_type";

    protected override bool AutoGenerateIdentity => false;

}
