using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class HouseBillInserterFactory : SingleIdInserterFactory<HouseBill.ToCreate>
{
    protected override string TableName => "house_bill";

    protected override bool AutoGenerateIdentity => false;

}
