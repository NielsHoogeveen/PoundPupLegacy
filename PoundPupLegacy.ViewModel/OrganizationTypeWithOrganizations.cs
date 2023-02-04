namespace PoundPupLegacy.ViewModel;

public record OrganizationTypeWithOrganizations
{
    public string OrganizationTypeName { get; set; }

    public Link[] Organizations { get; set; }
}
