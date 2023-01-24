namespace PoundPupLegacy.ViewModel;

public record OrganizationTypeWithOrganizations
{
    public string OrganizationTypeName { get; set; }

    public OrganizationListEntry[] Organizations { get; set; }
}
