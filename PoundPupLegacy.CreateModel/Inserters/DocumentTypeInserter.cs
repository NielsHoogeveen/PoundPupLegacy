namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class DocumentTypeInserterFactory : SingleIdInserterFactory<NewDocumentType>
{
    protected override string TableName => "document_type";

    protected override bool AutoGenerateIdentity => false;

}
