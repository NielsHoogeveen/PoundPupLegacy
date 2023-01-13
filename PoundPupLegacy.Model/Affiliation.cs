namespace PoundPupLegacy.Model;

public sealed record Affiliation
{
    public required int Id { get; init; }
    public required int OrganizationIdTo { get; init; }
    public required int OrganizationIdFrom { get; init; }
    public required int? ProofId { get; init; }
    public required int AffilitionTypeId { get; init; }
}
