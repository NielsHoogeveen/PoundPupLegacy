namespace PoundPupLegacy.CreateModel;

public sealed record NewPersonOrganizationRelationType : NewNameableBase, EventuallyIdentifiablePersonOrganizationRelationType
{
    public required bool HasConcreteSubtype { get; init; }
}
public sealed record ExistingPersonOrganizationRelationType : ExistingNameableBase, ImmediatelyIdentifiablePersonOrganizationRelationType
{
    public required bool HasConcreteSubtype { get; init; }
}
public interface ImmediatelyIdentifiablePersonOrganizationRelationType : PersonOrganizationRelationType, ImmediatelyIdentifiableNameable
{
}
public interface EventuallyIdentifiablePersonOrganizationRelationType : PersonOrganizationRelationType, EventuallyIdentifiableNameable
{
}
public interface PersonOrganizationRelationType : Nameable
{
    bool HasConcreteSubtype { get; }
}
