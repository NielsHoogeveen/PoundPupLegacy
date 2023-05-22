namespace PoundPupLegacy.CreateModel;

public sealed record NewUnitedStatesPoliticalParty : NewOrganizationBase, EventuallyIdentifiableUnitedStatesPoliticalParty
{
}
public sealed record ExistingUnitedStatesPoliticalParty : ExistingOrganizationBase, ImmediatelyIdentifiableUnitedStatesPoliticalParty
{
}
public interface ImmediatelyIdentifiableUnitedStatesPoliticalParty : UnitedStatesPoliticalParty, ImmediatelyIdentifiableOrganization
{
}
public interface EventuallyIdentifiableUnitedStatesPoliticalParty : UnitedStatesPoliticalParty, EventuallyIdentifiableOrganization
{
}
public interface UnitedStatesPoliticalParty : Organization
{
}
