namespace PoundPupLegacy.CreateModel;

public sealed record NewPartyPoliticalEntityRelationType : NewNameableBase, EventuallyIdentifiablePartyPoliticalEntityRelationType
{
    public required bool HasConcreteSubtype { get; init; }
}
public sealed record ExistingPartyPoliticalEntityRelationType : ExistingNameableBase, ImmediatelyIdentifiablePartyPoliticalEntityRelationType
{
    public required bool HasConcreteSubtype { get; init; }
}
public interface ImmediatelyIdentifiablePartyPoliticalEntityRelationType : PartyPoliticalEntityRelationType, ImmediatelyIdentifiableNameable
{
}
public interface EventuallyIdentifiablePartyPoliticalEntityRelationType : PartyPoliticalEntityRelationType, EventuallyIdentifiableNameable
{
}
public interface PartyPoliticalEntityRelationType : Nameable
{
    bool HasConcreteSubtype { get; }
}
