namespace PoundPupLegacy.CreateModel;

public sealed record NewBillActionType : NewNameableBase, EventuallyIdentifiableNameable
{
}
public sealed record ExistingBillActionType : ExistingNameableBase, ImmediatelyIdentifiableNameable
{
}
