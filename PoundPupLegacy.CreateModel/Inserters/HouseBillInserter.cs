namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class HouseBillInserterFactory : SingleIdInserterFactory<HouseBill.HouseBillToCreate>
{
    protected override string TableName => "house_bill";

    protected override bool AutoGenerateIdentity => false;

}
