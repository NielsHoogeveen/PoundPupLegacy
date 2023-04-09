namespace PoundPupLegacy.CreateModel;

public interface Organization : Party
{
    string? WebsiteUrl { get; }
    string? EmailAddress { get; }
    DateTimeRange? Established { get; }
    DateTimeRange? Terminated { get; }
    List<OrganizationOrganizationType> OrganizationTypes { get; }
}
