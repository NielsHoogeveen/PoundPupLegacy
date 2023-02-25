namespace PoundPupLegacy.Model;

public interface Organization :Party
{
    string? WebsiteURL { get; }
    string? EmailAddress { get;  }
    string Description { get; }
    DateTime? Established { get; }
    DateTime? Terminated { get; }
    List<OrganizationOrganizationType> OrganizationTypes { get; }
}
