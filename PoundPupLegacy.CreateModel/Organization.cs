namespace PoundPupLegacy.CreateModel;

public interface Organization : Party
{
    string? WebsiteUrl { get; }
    string? EmailAddress { get; }
    FuzzyDate? Established { get; }
    FuzzyDate? Terminated { get; }
    List<OrganizationOrganizationType> OrganizationTypes { get; }
}
