using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class PageInserterFactory : SingleIdInserterFactory<Page.ToCreate>
{
    protected override string TableName => "page";

    protected override bool AutoGenerateIdentity => false;

}
