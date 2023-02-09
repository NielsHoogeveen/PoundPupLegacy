namespace PoundPupLegacy.Model;

public record CasePartiesOrganization
{
    public required int CasePartiesId { get; init; }
    public required int OrganizationId { get; init; }
}
