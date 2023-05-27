namespace PoundPupLegacy.EditModel;

public record CasePartyTypeCaseParties
{
    public required int? Id { get; init; }
    public required int? CaseId { get; init; }
    public required int CasePartyTypeId { get; init; }
    public required string CasePartyTypeName { get; init; }
    public required string? OrganizationsText { get; init; }
    public required string? PersonsText { get; init; }

    private List<OrganizationCaseParty> organizations = new List<OrganizationCaseParty>();

    private List<PersonCaseParty> persons = new List<PersonCaseParty>();

    public List<OrganizationCaseParty> Organizations {
        get => organizations;
        init => organizations = value ?? new List<OrganizationCaseParty>();
    }
    public List<PersonCaseParty> Persons {
        get => persons;
        init => persons = value ?? new List<PersonCaseParty>();
    }
}
