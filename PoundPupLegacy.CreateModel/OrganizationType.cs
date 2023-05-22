namespace PoundPupLegacy.CreateModel;

public sealed record NewOrganizationType : NewNameableBase, EventuallyIdentifiableOrganizationType
{
    public required bool HasConcreteSubtype { get; init; }

}
public sealed record ExistingOrganizationType : ExistingNameableBase, ImmediatelyIdentifiableOrganizationType
{
    public required bool HasConcreteSubtype { get; init; }
}
public interface ImmediatelyIdentifiableOrganizationType : OrganizationType, ImmediatelyIdentifiableNameable
{
}
public interface EventuallyIdentifiableOrganizationType : OrganizationType, EventuallyIdentifiableNameable
{
}

public interface OrganizationType: Nameable
{
    bool HasConcreteSubtype { get; }
}