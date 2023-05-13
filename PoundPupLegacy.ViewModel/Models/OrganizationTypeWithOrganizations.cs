namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(OrganizationTypeWithOrganizations))]
public partial class OrganizationTypeWithOrganizationsJsonContext : JsonSerializerContext { }

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
