using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class SenateBillInserterFactory : SingleIdInserterFactory<SenateBill.ToCreate>
{
    protected override string TableName => "senate_bill";

    protected override bool AutoGenerateIdentity => false;

}
