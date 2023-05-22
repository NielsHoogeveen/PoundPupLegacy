namespace PoundPupLegacy.CreateModel;

public sealed record NewUnitedStatesPoliticalPartyAffliation : NewNameableBase, EventuallyIdentifiableUnitedStatesPoliticalPartyAffliation
{
    public required int? UnitedStatesPoliticalPartyId { get; init; }
}
public sealed record ExistingUnitedStatesPoliticalPartyAffliation : ExistingNameableBase, ImmediatelyIdentifiableUnitedStatesPoliticalPartyAffliation
{
    public required int? UnitedStatesPoliticalPartyId { get; init; }
}
public interface ImmediatelyIdentifiableUnitedStatesPoliticalPartyAffliation : UnitedStatesPoliticalPartyAffliation, ImmediatelyIdentifiableNameable
{
}
public interface EventuallyIdentifiableUnitedStatesPoliticalPartyAffliation : UnitedStatesPoliticalPartyAffliation, EventuallyIdentifiableNameable
{
}
public interface UnitedStatesPoliticalPartyAffliation : Nameable
{
    int? UnitedStatesPoliticalPartyId { get; }
}
