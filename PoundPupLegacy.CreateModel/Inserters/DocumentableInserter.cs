namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class DocumentableInserterFactory : SingleIdInserterFactory<Documentable>
{
    protected override string TableName => "documentable";

    protected override bool AutoGenerateIdentity => false;

}
