namespace PoundPupLegacy.DomainModel;

public sealed record CasePartiesOrganization : IRequest
{
    public required int CasePartiesId { get; init; }
    public required int OrganizationId { get; init; }
}
