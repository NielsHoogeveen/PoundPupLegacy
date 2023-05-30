namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class PageInserterFactory : SingleIdInserterFactory<Page.ToCreate>
{
    protected override string TableName => "page";

    protected override bool AutoGenerateIdentity => false;

}
