namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class PageInserterFactory : SingleIdInserterFactory<EventuallyIdentifiablePage>
{
    protected override string TableName => "page";

    protected override bool AutoGenerateIdentity => false;

}
