namespace PoundPupLegacy.CreateModel;

public sealed record NewDenomination : NewNameableBase, EventuallyIdentifiableDenomination
{
}
public sealed record ExistingDenomination : ExistingNameableBase, ImmediatelyIdentifiableDenomination
{
}
public interface ImmediatelyIdentifiableDenomination : Denomination, ImmediatelyIdentifiableNameable
{
}

public interface EventuallyIdentifiableDenomination : Denomination, EventuallyIdentifiableNameable
{
}

public interface Denomination: Nameable
{
}
