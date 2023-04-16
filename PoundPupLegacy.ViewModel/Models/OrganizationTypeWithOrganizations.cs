namespace PoundPupLegacy.ViewModel.Models;

public record OrganizationTypeWithOrganizations
{
    public required string OrganizationTypeName { get; init; }

    private Link[] organizations = Array.Empty<Link>();
    public required Link[] Organizations
    {
        get => organizations;
        init
        {
            if (value is not null)
            {
                organizations = value;
            }
        }
    }
}
