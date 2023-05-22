namespace PoundPupLegacy.CreateModel;
public interface ImmediatelyIdentifiableBill : Bill, ImmediatelyIdentifiableNameable, ImmediatelyIdentifiableDocumentable
{
}
public interface EventuallyIdentifiableBill: Bill, EventuallyIdentifiableNameable, EventuallyIdentifiableDocumentable
{
}

public interface Bill : Nameable, Documentable
{
    DateTime? IntroductionDate { get; }

    int? ActId { get; }

}

public abstract record NewBillBase: NewNameableBase, EventuallyIdentifiableBill
{
    public required DateTime? IntroductionDate { get; init; }

    public required int? ActId { get; init; }

}

public abstract record ExistingBillBase : ExistingNameableBase, ImmediatelyIdentifiableBill
{
    public required DateTime? IntroductionDate { get; init; }

    public required int? ActId { get; init; }

}
