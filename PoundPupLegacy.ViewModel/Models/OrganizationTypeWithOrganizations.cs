namespace PoundPupLegacy.ViewModel.Models;

public record OrganizationTypeWithOrganizations
{
    public required string OrganizationTypeName { get; init; }

    private BasicLink[] organizations = Array.Empty<BasicLink>();
    public required BasicLink[] Organizations {
        get => organizations;
        init {
            if (value is not null) {
                organizations = value;
            }
        }
    }
}
