namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class HouseBillInserterFactory : SingleIdInserterFactory<EventuallyIdentifiableHouseBill>
{
    protected override string TableName => "house_bill";

    protected override bool AutoGenerateIdentity => false;

}
