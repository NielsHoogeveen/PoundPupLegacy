namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class DocumentableInserterFactory : SingleIdInserterFactory<DocumentableToCreate>
{
    protected override string TableName => "documentable";

    protected override bool AutoGenerateIdentity => false;

}
