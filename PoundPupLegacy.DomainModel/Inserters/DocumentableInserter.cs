using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class DocumentableInserterFactory : SingleIdInserterFactory<DocumentableToCreate>
{
    protected override string TableName => "documentable";

    protected override bool AutoGenerateIdentity => false;

}
