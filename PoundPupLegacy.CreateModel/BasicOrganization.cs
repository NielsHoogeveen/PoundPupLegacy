namespace PoundPupLegacy.CreateModel;

public sealed record NewBasicOrganization : NewOrganizationBase, EventuallyIdentifiableOrganization
{
}
public sealed record ExistingOrganization : ExistingOrganizationBase, ImmediatelyIdentifiableOrganization
{
}
