namespace PoundPupLegacy.CreateModel;

public sealed record NewSenateBill : NewBillBase, EventuallyIdentifiableSenateBill
{
}
public sealed record ExistingSenateBill : ExistingBillBase, ImmediatelyIdentifiableSenateBill
{
}
public interface ImmediatelyIdentifiableSenateBill : SenateBill, ImmediatelyIdentifiableBill
{
}
public interface EventuallyIdentifiableSenateBill : SenateBill, EventuallyIdentifiableBill
{
}
public interface SenateBill : Bill
{
}
