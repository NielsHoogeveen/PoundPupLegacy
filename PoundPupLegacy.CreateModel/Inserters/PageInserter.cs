namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class PageInserterFactory : SingleIdInserterFactory<NewPage>
{
    protected override string TableName => "page";

    protected override bool AutoGenerateIdentity => false;

}
