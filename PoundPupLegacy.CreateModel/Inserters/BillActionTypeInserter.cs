namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BillActionTypeInserterFactory : SingleIdInserterFactory<BillActionType>
{
    protected override string TableName => "bill_action_type";

    protected override bool AutoGenerateIdentity => false;

}
