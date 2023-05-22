namespace PoundPupLegacy.CreateModel;
public interface ImmediatelyIdentifiableOrganization : Organization, ImmediatelyIdentifiableParty
{

}
public interface EventuallyIdentifiableOrganization : Organization, EventuallyIdentifiableParty
{

}
public interface Organization : Party
{
    string? WebsiteUrl { get; }
    string? EmailAddress { get; }
    FuzzyDate? Established { get; }
    FuzzyDate? Terminated { get; }
    List<OrganizationOrganizationType> OrganizationTypes { get; }
}

public abstract record NewOrganizationBase: NewNameableBase, EventuallyIdentifiableOrganization
{
    public required string? WebsiteUrl { get; init; }
    public required string? EmailAddress { get; init; }
    public required FuzzyDate? Established { get; init; }
    public required FuzzyDate? Terminated { get; init; }
    public required List<OrganizationOrganizationType> OrganizationTypes { get; init; }
}
public abstract record ExistingOrganizationBase : ExistingNameableBase, ImmediatelyIdentifiableOrganization
{
    public required string? WebsiteUrl { get; init; }
    public required string? EmailAddress { get; init; }
    public required FuzzyDate? Established { get; init; }
    public required FuzzyDate? Terminated { get; init; }
    public required List<OrganizationOrganizationType> OrganizationTypes { get; init; }
}