namespace PoundPupLegacy.CreateModel;

public record CasePartiesOrganization : IRequest
{
    public required int CasePartiesId { get; init; }
    public required int OrganizationId { get; init; }
}
