namespace PoundPupLegacy.CreateModel;

public sealed record NewBillActionType : NewNameableBase, EventuallyIdentifiableBillActionType
{
}
public sealed record ExistingBillActionType : ExistingNameableBase, ImmediatelyIdentifiableBillActionType
{
}
public interface ImmediatelyIdentifiableBillActionType : BillActionType, ImmediatelyIdentifiableNameable
{
}

public interface EventuallyIdentifiableBillActionType : BillActionType, EventuallyIdentifiableNameable
{
}

public interface BillActionType: Nameable
{
}
