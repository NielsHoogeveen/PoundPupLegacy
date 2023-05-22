namespace PoundPupLegacy.EditModel;

public record CasePartyTypeCaseParties
{
    public required int CasePartyTypeId { get; init; }

    public required string CasePartyTypeName { get; init; }

    public required string? OrganizationsText { get; init; }

    public required string? PersonsText { get; init; }

    private List<OrganizationListItem> organizations = new List<OrganizationListItem>();
    private List<PersonListItem> persons = new List<PersonListItem>();

    public List<OrganizationListItem> Organizations {
        get => organizations;
        init => organizations = value ?? new List<OrganizationListItem>();
    }
    public List<PersonListItem> Persons {
        get => persons;
        init => persons = value ?? new List<PersonListItem>();
    }
}
