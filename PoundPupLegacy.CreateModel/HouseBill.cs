namespace PoundPupLegacy.CreateModel;

public sealed record NewHouseBill : NewBillBase, EventuallyIdentifiableHouseBill
{
}
public sealed record ExistingHouseBill : ExistingBillBase, ImmediatelyIdentifiableHouseBill
{
}
public interface ImmediatelyIdentifiableHouseBill : HouseBill, ImmediatelyIdentifiableBill
{
}
public interface EventuallyIdentifiableHouseBill : HouseBill, EventuallyIdentifiableBill
{
}
public interface HouseBill : Bill
{
}
